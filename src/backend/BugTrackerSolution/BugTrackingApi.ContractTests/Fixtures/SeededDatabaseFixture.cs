using Alba.Security;
using BugTrackerApi.Services;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BugTrackingApi.ContractTests.Fixtures;
public class SeededDatabaseFixture : BaseAlbaFixture
{
    private readonly string _imageName = "jeffrygonzalez/bug-tracker-seeded:excel-rows";
    private readonly IContainer _container;
    public SeededDatabaseFixture()
    {
        _container = new ContainerBuilder()
          .WithEnvironment("PGDATA", "/pgdata")
          .WithPortBinding(5432, true)
          .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(".*database system is ready to accept connections*"))
          .WithImage(_imageName).Build();

    }

    protected override async Task Initializeables()
    {

        await _container.StartAsync().ConfigureAwait(false);
     
        var port = _container.GetMappedPublicPort("5432");
        var host = _container.Hostname;
        //await Task.Delay(1500);
        Console.WriteLine($"Port {port} host {host}");
        Environment.SetEnvironmentVariable("ConnectionStrings__bugs", $"PORT = {port}; HOST = {host}; DATABASE = postgres; PASSWORD = password; USER ID = postgres; sslmode = disable");
        // Need to tell it to use THIS container instead of the one in our appsetting.development.json
    }
    protected override async Task Disposables()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__bugs", null);
        // Use whatever database library to delete whatever was created by this "collection" of tests.

    }

    protected override void RegisterServices(IServiceCollection services)
    {


    }
    protected override AuthenticationStub GetStub()
    {
        return base.GetStub().WithName("Steve");
    }

}
