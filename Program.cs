using ExemploApiNetBasica.Data;
using ExemploApiNetBasica.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o Contexto do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Já está certo (usando Npgsql)

// 2. Configurar o Swagger (documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. (IMPORTANTE) Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://SEU-APP-VUE-VAI-AQUI.vercel.app") // Já pensando no futuro
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- MUDANÇA 1: EXECUTAR MIGRAÇÕES AUTOMATICAMENTE ---
// Este bloco de código executa o "dotnet ef database update" para nós
// toda vez que a API é iniciada no servidor.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Aplica qualquer migração pendente (cria o banco de dados e as tabelas)
    dbContext.Database.Migrate();
}
// --- FIM DA MUDANÇA 1 ---


// Configurar o pipeline do app

// --- MUDANÇA 2: MOVER O SWAGGER PARA FORA DO "IF" ---
// Queremos ver nossa documentação no Render (Produção)
app.UseSwagger();
app.UseSwaggerUI();
// --- FIM DA MUDANÇA 2 ---

if (app.Environment.IsDevelopment())
{
    // O 'if' agora pode ficar vazio, ou você pode deletá-lo
}

// app.UseHttpsRedirection(); // CORRETO: Já removemos isso

// 4. (IMPORTANTE) Aplicar a política de CORS
app.UseCors("AllowVueApp");


// --- NOSSOS ENDPOINTS (A API DE FATO) ---
// (Nenhuma mudança necessária aqui)

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