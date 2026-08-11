using EliteCarAPI.Data;
using EliteCarAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────
// Banco de dados — PostgreSQL via Supabase (Npgsql)
// Connection string configurada em appsettings.Development.json
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

// ──────────────────────────────────────────────────────────
// OpenAPI — geração do documento JSON (built-in .NET 10)
// Swagger UI renderiza o documento em /swagger
// ──────────────────────────────────────────────────────────
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, _) =>
    {
        document.Info = new OpenApiInfo
        {
            Title       = "EliteCar API",
            Version     = "v1",
            Description = """
                API REST para gerenciamento de clientes, estoque de veículos e vendas da concessionária **EliteCar**.

                ## Recursos
                - **Clientes** — cadastro, listagem, busca por CPF, atualização e exclusão lógica
                - **Carros** — cadastro, listagem, busca por placa, atualização e exclusão lógica
                - **Pedidos de Venda** — cadastro, listagem, busca por data, atualização e exclusão lógica

                ## Regras de negócio aplicadas automaticamente
                - Desconto de **5%** para vendas à vista
                - Valor mínimo = preço do veículo para financiamento/consórcio
                - Status do veículo atualizado automaticamente após venda ou cancelamento
                - Exclusão lógica em todas as entidades (campo `ativo`)
                """,
            Contact = new OpenApiContact
            {
                Name  = "EliteCar Dev Team",
                Email = "dev@elitecar.com"
            }
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// ──────────────────────────────────────────────────────────
// Swagger UI — habilitado em todos os ambientes
// • Desenvolvimento : https://localhost:{porta}/swagger
// • Produção (Render): https://elitecar-api.onrender.com/swagger
// ──────────────────────────────────────────────────────────

// Gera o documento em /openapi/v1.json
app.MapOpenApi();

// Swagger UI aponta para o documento gerado acima
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/openapi/v1.json", "EliteCar API v1");
    c.RoutePrefix          = "swagger";
    c.DocumentTitle        = "EliteCar API — Swagger UI";
    c.DefaultModelsExpandDepth(1);
    c.DisplayRequestDuration();
});

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
