using ExemploApiNetBasica.Data;
using ExemploApiNetBasica.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- NOVO (Corrigido): TRADUÇÃO DA CONNECTION STRING ---
// 1. Pega a string de conexão (URL) do Render
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
string npgsqlConnectionString = "";

if (!string.IsNullOrEmpty(connectionString))
{
    try
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');
        var user = userInfo[0];
        var password = userInfo[1];
        var host = uri.Host;

        // --- A CORREÇÃO ESTÁ AQUI ---
        // Se a porta não for especificada na URL, uri.Port retorna -1.
        // O PostgreSQL usa a porta 5432 por padrão.
        var port = uri.Port > 0 ? uri.Port : 5432;
        // --- FIM DA CORREÇÃO ---

        var database = uri.AbsolutePath.TrimStart('/');

        npgsqlConnectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};SSLMode=Require;TrustServerCertificate=true";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao parsear a connection string: {ex.Message}");
        // Trava a aplicação se a string for inválida mas não nula
        throw new InvalidOperationException("Não foi possível parsear a connection string do BD.", ex);
    }
}


// 1. Configurar o Contexto do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(npgsqlConnectionString)); 

// 2. Configurar o Swagger (documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. (IMPORTANTE) Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://SEU-APP-VUE-VAI-AQUI.vercel.app") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Executa as migrações automaticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configurar o pipeline do app
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // O 'if' agora pode ficar vazio
}

// app.UseHttpsRedirection(); // Correto, continua removido

app.UseCors("AllowVueApp");


// --- ENDPOINTS (Sem mudanças) ---
// (O código dos MapGet, MapPost, MapPut, MapDelete fica aqui)

// GET: /api/tarefas
app.MapGet("/api/tarefas", async (AppDbContext context) =>
{
    var tarefas = await context.Tarefas.ToListAsync();
    return Results.Ok(tarefas);
});

// POST: /api/tarefas
app.MapPost("/api/tarefas", async (AppDbContext context, Tarefa tarefa) =>
{
    await context.Tarefas.AddAsync(tarefa);
    await context.SaveChangesAsync();
    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
});

// PUT: /api/tarefas/{id}
app.MapPut("/api/tarefas/{id}", async (AppDbContext context, int id) =>
{
    var tarefaEncontrada = await context.Tarefas.FindAsync(id);
    if (tarefaEncontrada == null)
    {
        return Results.NotFound();
    }
    tarefaEncontrada.Concluida = !tarefaEncontrada.Concluida;
    await context.SaveChangesAsync();
    return Results.Ok(tarefaEncontrada);
});

// DELETE: /api/tarefas/{id}
app.MapDelete("/api/tarefas/{id}", async (AppDbContext context, int id) =>
{
    var tarefaEncontrada = await context.Tarefas.FindAsync(id);
    if (tarefaEncontrada == null)
    {
        return Results.NotFound();
    }
    context.Tarefas.Remove(tarefaEncontrada);
    await context.SaveChangesAsync();
    return Results.NoContent();
});


app.Run();