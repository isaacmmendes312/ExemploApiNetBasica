using ExemploApiNetBasica.Data;
using ExemploApiNetBasica.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o Contexto do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configurar o Swagger (documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. (IMPORTANTE) Adicionar CORS para permitir que o Vue (do localhost) acesse a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // A porta padrão do Vue
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configurar o pipeline do app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 4. (IMPORTANTE) Aplicar a política de CORS
app.UseCors("AllowVueApp");


// --- NOSSOS ENDPOINTS (A API DE FATO) ---

// GET: /api/tarefas (Buscar todas as tarefas)
app.MapGet("/api/tarefas", async (AppDbContext context) =>
{
    var tarefas = await context.Tarefas.ToListAsync();
    return Results.Ok(tarefas);
});

// POST: /api/tarefas (Criar uma nova tarefa)
app.MapPost("/api/tarefas", async (AppDbContext context, Tarefa tarefa) =>
{
    await context.Tarefas.AddAsync(tarefa);
    await context.SaveChangesAsync();
    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
});

app.MapPut("/api/tarefas/{id}", async (AppDbContext context, int id) =>
{
    // 1. Tenta encontrar a tarefa no banco de dados pelo 'id'
    var tarefaEncontrada = await context.Tarefas.FindAsync(id);

    // 2. Se não encontrar, retorna um erro 404 (Not Found)
    if (tarefaEncontrada == null)
    {
        return Results.NotFound();
    }

    // 3. A LÓGICA: Inverte o valor de 'Concluida'
    // Se era 'false', vira 'true'. Se era 'true', vira 'false'.
    tarefaEncontrada.Concluida = !tarefaEncontrada.Concluida;

    // 4. Salva a mudança no banco de dados
    await context.SaveChangesAsync();

    // 5. Retorna a tarefa atualizada com um status 200 (OK)
    return Results.Ok(tarefaEncontrada);
});

// NOVO ENDPOINT: DELETE (CORRIGIDO)
// REMOVI O "=>" EXTRA DESTA LINHA
app.MapDelete("/api/tarefas/{id}", async (AppDbContext context, int id) => 
    {
    // 1. Encontrar a  tarefa no banco de dados
    var tarefaEncontrada = await context.Tarefas.FindAsync(id);

    // 2. Caso contrário, retorna um erro (Not Found)
    if (tarefaEncontrada == null)
    {
        return Results.NotFound();
    }

    // 3. Remove a tarefa
    context.Tarefas.Remove(tarefaEncontrada);

    // 4. Salva a mudança no banco de dados
        await
        context.SaveChangesAsync();

    //5. Retorna um status 204 (No content)
        return Results.NoContent();
    });

// } // REMOVI A CHAVE "}" EXTRA QUE ESTAVA AQUI

app.Run();