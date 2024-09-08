using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Context
{
    public class TarefaContext: DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options): base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
