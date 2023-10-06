using Alba;
using BugTrackerApi.Models;
using BugTrackerApi.Services;
using BugTrackingApi.ContractTests.Fixtures;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace BugTrackingApi.ContractTests.BugReports;

[Collection("FilingABugReport")]
public class FilingAndRetreivingABugReport
{
    private readonly IAlbaHost _host;
 

    public FilingAndRetreivingABugReport(FilingBugReportFixture fixture)
    {
        fixture.AddDummyDesktopSupportStub();
        _host = fixture.AlbaHost;

    }

    [Fact]
    public async Task AddingAndRetrievingABugReort()
    {
        // Given 
      
        var request = new BugReportCreateRequest
        {
            Description = "spell checker broken",
            Narrative = "I can know lownger chek my slepping!"
        };

        var response = await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/catalog/excel/bugs");
            api.StatusCodeShouldBe(201);
        });

        var firstResponse = response.ReadAsJson<BugReportCreateResponse>();
        Assert.NotNull(firstResponse);

        // When
        var response2 = await _host.Scenario(api =>
        {
            api.Get.Url($"/catalog/excel/bugs/{firstResponse.Id}");
            api.StatusCodeShouldBeOk();
        });

        var secondResponse = response2.ReadAsJson<BugReportCreateResponse>();

        // Then
        Assert.NotNull(secondResponse);

        Assert.Equal(firstResponse, secondResponse);
    }

}
