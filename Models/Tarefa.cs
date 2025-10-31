namespace ExemploApiNetBasica.Models
{
    public class Tarefa
    {
        public int Id { get; set; } // Chave primária
        public string? Titulo { get; set; }
        public bool Concluida { get; set; }
    }
}