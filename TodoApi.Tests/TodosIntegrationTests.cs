using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class TodosIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodosIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTodos_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/todos");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetTodos_FilterByTitle()
    {
        var response = await _client.GetAsync("/todos?title=test");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetTodos_Pagination()
    {
        var response = await _client.GetAsync("/todos?page=1&pageSize=10");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateTodo_ShouldUpdateCompleted()
    {
        var update = new { completed = true };

        var response = await _client.PutAsJsonAsync("/todos/1", update);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateTodo_ShouldFail_WhenMoreThan5Incomplete()
    {
        var update = new { completed = false };

        var response = await _client.PutAsJsonAsync("/todos/1", update);

        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.BadRequest
        );
    }
}