using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Integration.ExternalServices.OpenLibrary.Clients;
public class OpenLibraryClientTests
{
    private OpenLibraryClient CreateClient(HttpMessageHandler handler)
    {
        var settings = new OpenLibrarySettings
        {
            BaseUrl = "https://openlibrary.org",
            CoverBaseUrl = "https://covers.openlibrary.org/b/"
        };

        var httpClient = new HttpClient(handler);

        var optionsMock = new Mock<IOptions<OpenLibrarySettings>>();
        optionsMock.Setup(o => o.Value).Returns(settings);

        return new OpenLibraryClient(httpClient, optionsMock.Object);
    }

    [Fact]
    public async Task SearchBooksAsync_ReturnsDto_WhenResponseOk()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(new { }),
            });

        var client = CreateClient(handlerMock.Object);

        var result = await client.SearchBooksAsync("tolkien", 1, 10);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetCoverUrl_ReturnsCorrectUrl()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        var client = CreateClient(handlerMock.Object);

        var url = client.GetCoverUrl("OL123M", "L");

        Assert.Equal("https://covers.openlibrary.org/b/OL123M-L.jpg", url);
    }

    [Fact]
    public async Task GetAuthorBioAsync_ReturnsDto_WhenResponseOk()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(new { }),
            });

        var client = CreateClient(handlerMock.Object);

        var result = await client.GetAuthorBioAsync("OL1A");

        Assert.NotNull(result);
    }
}
