using Alba;
using BugTrackerApi.Models;
using BugTrackingApi.ContractTests.Fixtures;

namespace BugTrackingApi.ContractTests.BugReports;

// [Collection("SeededDatabaseCollection")]
public class GettingAllBugReportsForAScenario : IClassFixture<SeededDatabaseFixture>
{
    private readonly IAlbaHost _host;
    public GettingAllBugReportsForAScenario(SeededDatabaseFixture fixture)
    {
        _host = fixture.AlbaHost;
    }

    [Fact]
    public async Task GetAllBugsForExcel()
    {

        var response = await _host.Scenario(api =>
        {
            api.Get.Url("/catalog/excel/bugs");
            api.StatusCodeShouldBeOk();
        });

        var data = response.ReadAsJson<List<BugReportCreateResponse>>();

        Assert.NotNull(data);

        Assert.Equal(3, data.Count);

        var third = data.Where(d => d.Id == "when-i-sum-up-a-column-it-shows-in-the-total").SingleOrDefault();

        Assert.NotNull(third);
        // what are you going to look at?

    }
}
