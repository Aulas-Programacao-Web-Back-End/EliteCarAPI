using EliteCarAPI.Data;
using EliteCarAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────
// Banco de dados — PostgreSQL via Supabase (Npgsql)
// Connection string configurada em appsettings.json
// ──────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ──────────────────────────────────────────────────────────
// Services (camada de regras de negócio — MVC)
// ──────────────────────────────────────────────────────────
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<CarroService>();
builder.Services.AddScoped<PedidoVendaService>();

// ──────────────────────────────────────────────────────────
// Controllers + validação automática via DataAnnotations
// ──────────────────────────────────────────────────────────
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
