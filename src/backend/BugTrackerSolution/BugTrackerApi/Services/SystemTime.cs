namespace BugTrackerApi.Services;

public interface ISystemTime
{
    DateTimeOffset GetCurrent();
}

public class SystemTime : ISystemTime
{
    public DateTimeOffset GetCurrent() => DateTimeOffset.UtcNow;
}
