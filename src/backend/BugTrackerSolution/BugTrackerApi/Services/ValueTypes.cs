using BugTrackerApi.Models;

using OneOf;

namespace BugTrackerApi.Services;

public record SoftwareNotInCatalog();
public record BugReportNotFound();

public record BugReportList(IReadOnlyList<BugReportCreateResponse> data);

[GenerateOneOf]
public partial class SoftwareEntityOrNotInCatalog : OneOfBase<SoftwareEntity, SoftwareNotInCatalog>
{

}

public record SupportTicketRequest
{
    public string Software { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
}

public record SupportTicketResponse
{
    public Guid TicketId { get; set; }
    public SupportTicketRequest Request { get; set; } = new();
}