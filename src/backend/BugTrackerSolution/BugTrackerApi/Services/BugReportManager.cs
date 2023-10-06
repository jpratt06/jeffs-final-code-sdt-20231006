using BugTrackerApi.Models;

using Marten;

using Microsoft.CodeAnalysis.VisualBasic.Syntax;

using OneOf;

using SlugUtils;

namespace BugTrackerApi.Services;

public class BugReportManager
{
    private readonly SoftwareCatalogManager _softwareCatalog;
    private readonly ISystemTime _systemTime;
    private readonly SlugGenerator _slugGenerator;
    private readonly IDocumentSession _documentSession;
    private readonly IDesktopSupportHttpClient _desktopSupportHttpClient;
    private readonly ILogger<BugReportManager> _logger;

    public BugReportManager(SoftwareCatalogManager softwareCatalog, ISystemTime systemTime, SlugGenerator slugGenerator, IDocumentSession documentSession, IDesktopSupportHttpClient desktopSupportHttpClient, ILogger<BugReportManager> logger)
    {
        _softwareCatalog = softwareCatalog;
        _systemTime = systemTime;
        _slugGenerator = slugGenerator;
        _documentSession = documentSession;
        _desktopSupportHttpClient = desktopSupportHttpClient;
        _logger = logger;
    }



    public async Task<OneOf<BugReportCreateResponse, SoftwareNotInCatalog>> CreateBugReportAsync(string user, string software, BugReportCreateRequest request)
    {
        var softwareLookup = await _softwareCatalog.IsSoftwareInCatalogAsync(software);
      
        if (softwareLookup.IsT1)
        {
            return new SoftwareNotInCatalog();
        }

        var report = new BugReportCreateResponse
        {
            Created = _systemTime.GetCurrent(),
            Id = await _slugGenerator.GenerateSlugAsync(request.Description, CheckForUniqueAsync),
            Issue = request,
            Software = ((SoftwareEntity)softwareLookup).Name,
            Status = IssueStatus.InTriage,
            User = user
        };

        var entityToSave = new BugReportEntity
        {
            Id = Guid.NewGuid(),
            BugReport = report,
        };
        _documentSession.Insert(entityToSave);
        await _documentSession.SaveChangesAsync();
        // send a request to a remote API to tell them to assign this to a support person.
        var apiResponse = await _desktopSupportHttpClient.SendSupportTicketAsync(new SupportTicketRequest
        {
            Software = software,
            User = user
        });

        _logger.LogInformation($"Got a ticket of {apiResponse.TicketId} for the issue {report.Id}");
        return report;





        async Task<bool> CheckForUniqueAsync(string slug)
        {
            return await _documentSession.Query<BugReportEntity>().Where(b => b.BugReport.Id == slug).AnyAsync() == false;
        }

    }

    public async Task<OneOf<BugReportCreateResponse, BugReportNotFound>> GetBugReportByIdAsync(string id)
    {
        var savedReport = await _documentSession.Query<BugReportEntity>().Where(b => b.BugReport.Id == id).SingleOrDefaultAsync();
        if (savedReport is not null)
        {
            return savedReport.BugReport;
        }
        else
        {
            return new BugReportNotFound();
        }
    }

    public async Task<OneOf<BugReportList, BugReportNotFound>> GetBugsForSoftwareAsync(string software)
    {
        var isInCatalog = await _softwareCatalog.IsSoftwareInCatalogAsync(software);

        if (isInCatalog.IsT1)
        {
            return new BugReportNotFound();
        }
        else
        {


            var response = await _documentSession.Query<BugReportEntity>()
                .Where(b => b.BugReport.Software == ((SoftwareEntity)isInCatalog).Name)
                .Select(b => b.BugReport)
                .ToListAsync();
            return new BugReportList(response);
        }

    }
}



