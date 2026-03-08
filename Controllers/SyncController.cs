using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("sync")]
public class SyncController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly HttpClient _http;

    public SyncController(AppDbContext context)
    {
        _context = context;
        _http = new HttpClient();
    }

    [HttpPost]
    public async Task<IActionResult> Sync()
    {
        var response = await _http.GetFromJsonAsync<List<Todo>>(
            "https://jsonplaceholder.typicode.com/todos"
        );

        if (response == null)
            return BadRequest();

        foreach (var todo in response)
        {
            if (!_context.Todos.Any(t => t.Id == todo.Id))
                _context.Todos.Add(todo);
        }

        await _context.SaveChangesAsync();

        return Ok("Dados sincronizados");
    }
}