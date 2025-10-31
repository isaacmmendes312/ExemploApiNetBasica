// 1. ESTA LINHA IMPORTA O ENTITY FRAMEWORK CORE
// Ela corrige: DbContext, DbContextOptions, base, DbSet
using Microsoft.EntityFrameworkCore;

// 2. ESTA LINHA IMPORTA SEU MODELO "TAREFA"
// Ela corrige: Tarefa (dentro de DbSet<Tarefa>)
using ExemploApiNetBasica.Models;

namespace ExemploApiNetBasica.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // O construtor fica aqui, sem mudanças
        }

        // Diz ao EF Core que existe uma "tabela" chamada Tarefas
        public DbSet<Tarefa> Tarefas { get; set; }
    }
}