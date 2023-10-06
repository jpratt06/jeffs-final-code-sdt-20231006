using Alba;
using BugTrackerApi.Models;
using BugTrackerApi.Services;

using BugTrackingApi.ContractTests.Fixtures;

using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace BugTrackingApi.ContractTests.BugReports;

[Collection("FilingABugReport")]
public class FilingABugReport
{

    private readonly IAlbaHost _host;
    private readonly ILogger<BugReportManager>? _logger;
    public FilingABugReport(FilingBugReportFixture fixture)
    {
        fixture.AddDummyDesktopSupportStub();
        _logger = fixture.BugReportLogger;
        _host = fixture.AlbaHost;
    }
    [Theory]
    [MemberData(nameof(GetSamplesForTheory))]
    public async Task FilingANewBugReport(string software, BugReportCreateRequest request, BugReportCreateResponse expectedReponse)
    {

        // When (and some then)
        var response = await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl($"/catalog/{software}/bugs");
            api.StatusCodeShouldBe(201);
            api.Header(HeaderNames.Location).ShouldHaveOneNonNullValue();
        });

        // Then
        var actualResponse = response.ReadAsJson<BugReportCreateResponse>();
        Assert.NotNull(actualResponse);

        Assert.Equal(expectedReponse, actualResponse);

        var header = response.Context.Response.Headers.Location.First();
        var expectedHeader = $"http://localhost/catalog/{software}/bugs/{actualResponse.Id}";
        Assert.Equal(expectedHeader, header);

        Assert.NotNull(_logger);
    
        _logger.Received().LogInformation($"Got a ticket of 38cba817-d6bc-463d-a4c8-c789dc8e72bc for the issue {actualResponse.Id}");

    }

    public static IEnumerable<object[]> GetSamplesForTheory()
    {
        var request1 = new BugReportCreateRequest()
        {
            Description = "Tacos Are Good",
            Narrative = "A big long thing with steps to reproduce"
        };
        var response1 = new BugReportCreateResponse
        {

            Id = "tacos-are-good", // Slug
            User = "Steve",
            Issue = request1,
            Status = IssueStatus.InTriage,
            Software = "Microsoft Excel",
            Created = FilingBugReportFixture.AssumedTime
        };

        var request2 = new BugReportCreateRequest()
        {
            Description = "Can't Install Extensions",
            Narrative = "VSCode is Broken"
        };
        var response2 = new BugReportCreateResponse
        {
            Id = "cant-install-extensions",
            User = "Steve",
            Status = IssueStatus.InTriage,
            Software = "Visual Studio Code",
            Created = FilingBugReportFixture.AssumedTime,
            Issue = request2
        };

        yield return new object[] { "excel", request1, response1 };
        yield return new object[] { "code", request2, response2 };

    }
}
