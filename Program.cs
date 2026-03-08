using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração da Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Configuração do DbContext para MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// --- ADICIONADO: Configuração do CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        p => p.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
// ---------------------------------------

builder.Services.AddControllers();
builder.Services.AddOpenApi(); 

var app = builder.Build();

// --- ADICIONADO: Ativação do Middleware do CORS ---
// Importante: app.UseCors deve vir ANTES de app.MapControllers
app.UseCors("AllowAll");
// -------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

public partial class Program { }