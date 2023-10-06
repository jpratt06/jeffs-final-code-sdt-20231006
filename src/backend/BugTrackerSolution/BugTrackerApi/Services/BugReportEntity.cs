using BugTrackerApi.Models;

namespace BugTrackerApi.Services;

public class BugReportEntity
{
    public Guid Id { get; set; }
    public BugReportCreateResponse BugReport { get; set; } = new();
}
