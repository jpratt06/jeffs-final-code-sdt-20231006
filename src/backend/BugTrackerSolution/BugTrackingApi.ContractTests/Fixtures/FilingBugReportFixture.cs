using Alba.Security;
using BugTrackerApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NSubstitute;
using Testcontainers.PostgreSql;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace BugTrackingApi.ContractTests.Fixtures;
public class FilingBugReportFixture : BaseAlbaFixture
{
    public static DateTimeOffset AssumedTime = new(new DateTime(1969, 4, 20, 23, 59, 59), TimeSpan.FromHours(-4));
    private readonly string PG_IMAGE = "postgres:15.2-bullseye";
    private readonly PostgreSqlContainer _pgContainer;
    public readonly WireMockServer MockServer;
    public ILogger<BugReportManager>? BugReportLogger;
    public FilingBugReportFixture()
    {
        MockServer = WireMockServer.Start(1341);
        _pgContainer = new PostgreSqlBuilder()
            .WithUsername("postgres")
            .WithPassword("password")
            .WithImage(PG_IMAGE).Build();

    }

    public void AddDummyDesktopSupportStub()
    {
        var stubbedBody = """
            {
               "ticketId": "38cba817-d6bc-463d-a4c8-c789dc8e72bc",
               "request": {
                "software": "stubbed",
                "user": "stubbed"
               }
            }
            """;

        MockServer
            .Given(Request.Create().WithPath("/support-tickets").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(201).WithBody(stubbedBody));
    }
    protected override async Task Initializeables()
    {
      
        await _pgContainer.StartAsync();
        // Need to tell it to use THIS container instead of the one in our appsetting.development.json
        Environment.SetEnvironmentVariable("ConnectionStrings__bugs", _pgContainer.GetConnectionString());

    }

    protected override async Task Disposables()
    {
        MockServer.Stop();
        MockServer.Dispose();
        await _pgContainer.DisposeAsync().AsTask();
        Environment.SetEnvironmentVariable("ConnectionStrings__bugs", null);
        // Use whatever database library to delete whatever was created by this "collection" of tests.

    }

    protected override void RegisterServices(IServiceCollection services)
    {
        BugReportLogger = Substitute.For<ILogger<BugReportManager>>();
        var fakeClock = Substitute.For<ISystemTime>();
        fakeClock.GetCurrent().Returns(AssumedTime);
        // The "recommendation" is you remove the existing thing first, I have NEVER had a problem when I don't.
        services.AddSingleton<ISystemTime>(fakeClock);
        services.AddSingleton<ILogger<BugReportManager>>(BugReportLogger);

    }
    protected override AuthenticationStub GetStub()
    {
        return base.GetStub().WithName("Steve");
    }

}
