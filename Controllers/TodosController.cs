using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("todos")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodosController(AppDbContext context)
    {
        _context = context;
    }

    // GET /todos
    [HttpGet]
    public async Task<IActionResult> GetTodos(
        int page = 1,
        int pageSize = 10,
        string? title = null,
        string sort = "id",
        string order = "asc")
    {
        var query = _context.Todos.AsQueryable();

        if (!string.IsNullOrEmpty(title))
            query = query.Where(t => t.Title.Contains(title));

        if (sort == "title")
            query = order == "asc"
                ? query.OrderBy(t => t.Title)
                : query.OrderByDescending(t => t.Title);
        else
            query = query.OrderBy(t => t.Id);

        var todos = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(todos);
    }

    // GET /todos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);

        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    // POST /todos
    [HttpPost]
    public async Task<IActionResult> CreateTodo(CreateTodoDto dto)
    {
        var incompleteCount = await _context.Todos
            .Where(t => t.UserId == dto.UserId && t.Completed == false)
            .CountAsync();

        if (incompleteCount >= 5)
        {
            return BadRequest(new
            {
                message = "O usuário já possui 5 tarefas incompletas."
            });
        }

        var todo = new Todo
        {
            Title = dto.Title,
            Completed = dto.Completed,
            UserId = dto.UserId
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    // PUT /todos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto dto)
    {
        var todo = await _context.Todos.FindAsync(id);

        if (todo == null)
            return NotFound();

        // Se estiver tentando deixar a tarefa como incompleta
        if (dto.Completed == false)
        {
            var incompleteCount = await _context.Todos
                .Where(t => t.UserId == todo.UserId && t.Completed == false && t.Id != id)
                .CountAsync();

            if (incompleteCount >= 5)
            {
                return BadRequest(new
                {
                    message = "O usuário já possui 5 tarefas incompletas. Complete uma antes de adicionar outra."
                });
            }
        }

        todo.Completed = dto.Completed;

        await _context.SaveChangesAsync();

        return Ok(todo);
    }
}