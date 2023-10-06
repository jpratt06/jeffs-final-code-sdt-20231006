using System.Net;
using BugTrackerApi.Services;
using Newtonsoft.Json;
using NSubstitute;

namespace BugTrackerApi.UnitTests;
public class DesktopSupportApiClientTests
{
    [Fact]
    public async Task UnitTestingByMockingTheHttpClientsMessageHandler()
    {
        // GIVEN
        var fakeTicket = Guid.NewGuid();
        // The class below - the Send method is protected, so we create a public version we can get to.
        var fakeHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK);
        // Just a response to send so we can make sure our SUT (the DesktopSupportHttpClient) is returning the response.
        var fakeRequestContent = new SupportTicketRequest()
        {
            Software = "excel",
            User = "paul"
        };
        var fakeResponseContent = new SupportTicketResponse()
        {
            Request = fakeRequestContent,
            TicketId = fakeTicket
        };

        // Add the JSON version of that response to the content.
        fakeResponse.Content = new StringContent(JsonConvert.SerializeObject(fakeResponseContent));
        // Stub It Out
        fakeHandler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
            .Returns(fakeResponse);
        var client = new HttpClient(fakeHandler); // come back to this.
        client.BaseAddress = new Uri("http://tacos.com");

        var desktopSupport = new DesktopSupportHttpClient(client);

        var response = await desktopSupport.SendSupportTicketAsync(fakeRequestContent);

        Assert.Equal(fakeTicket, response.TicketId);
        Assert.Equal(fakeRequestContent, response.Request);


    }
}

public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(MockSend(request, cancellationToken));
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return MockSend(request, cancellationToken);
    }

    public virtual HttpResponseMessage MockSend(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return null!;
    }
}