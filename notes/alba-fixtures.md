# Some Reference Alba Fixtures


## Base Fixture with Template Pattern

```csharp

using Alba;
using Alba.Security;
using Microsoft.Extensions.DependencyInjection;


namespace BugTrackerApi.ContractTests.Fixtures;
public abstract class BaseAlbaFixture : IAsyncLifetime
{

    public IAlbaHost AlbaHost = null!;
    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }

    public async Task InitializeAsync()
    {

        AlbaHost = await Alba.AlbaHost.For<Program>(builder => builder.ConfigureServices(services => RegisterServices(services)), GetStub());
    }

    protected abstract void RegisterServices(IServiceCollection services);
    protected virtual Task? Initializeables() { return default; }
    protected virtual Task? Disposables() { return default; }
    protected virtual AuthenticationStub GetStub()
    {
        return new AuthenticationStub();
    }
}

```

### An Application of the Base Fixture

```csharp
using Alba.Security;
using BugTrackerApi.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BugTrackerApi.ContractTests.Fixtures;

public class FilingABugFixture : BaseAlbaFixture
{
    public static DateTimeOffset AssumedTime = new(new DateTime(1969, 4, 20, 23, 59, 59), TimeSpan.FromHours(-4));

    protected override void RegisterServices(IServiceCollection services)
    {
        ISystemTime fakeClock = Substitute.For<ISystemTime>();
        fakeClock.GetCurrent().Returns(AssumedTime);
        services.AddSingleton<ISystemTime>(fakeClock);
    }

    protected override AuthenticationStub GetStub()
    {
        return base.GetStub().WithName("carl");
    }
}
```

### Creating a Collection Definition

```csharp
namespace BugTrackerApi.ContractTests.Fixtures;

[CollectionDefinition("happy path")]
public class HappyPathFixture : ICollectionFixture<FilingABugFixture>
{
}
```

## (Truncated) Example of using A Collection Definition

```csharp
[Collection("happy path")]
public class FilingABug 

{
    private readonly IAlbaHost _host;

    public FilingABug(FilingABugFixture fixture)
    {
        _host = fixture.AlbaHost;
    }
}

```