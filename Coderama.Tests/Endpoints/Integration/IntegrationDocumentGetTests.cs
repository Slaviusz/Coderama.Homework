namespace Coderama.Tests.Endpoints.Integration;

using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

public class IntegrationDocumentGetTests : IClassFixture<WebApplicationFactory<Program>> {

    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationDocumentGetTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test_DocumentGet_ShouldReturnDocument()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/documents/${Guid.NewGuid()}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
    }

    [Fact]
    public async Task Test_DocumentPost_ShouldReturnCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new {
            id = Guid.NewGuid().ToString(),
            tags = (string[])["important", ".net"],
            data = new {
                some = "data",
                optional = "fields"
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/documents", payload);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
    }

    [Fact]
    public async Task Test_DocumentPut_ShouldReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new {
            id = Guid.NewGuid().ToString(),
            tags = (string[])["important", ".net"],
            data = new {
                some = "data",
                optional = "fields"
            }
        };

        // Act
        var response = await client.PutAsJsonAsync($"/documents/${Guid.NewGuid()}", payload);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
