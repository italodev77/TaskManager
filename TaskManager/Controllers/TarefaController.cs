using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Context;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class TarefaController(TarefaContext context) : ControllerBase
    {
        private readonly TarefaContext _context = context;

        [HttpGet("{id}")]
        public IActionResult GetTarefa(int id)
        {
            var tarefas = _context.Tarefas.Find(id);   
            if (tarefas == null)
            {
                return NotFound();
            }

            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult CriarTarefa( Tarefa tarefa)
        {
            _context.Add(tarefa);
            _context.SaveChanges();
            return Ok(tarefa); 
        }

        [HttpDelete("{id}")]

        public IActionResult DeletarTarefa(int id)
        {
            var tarefa = _context.Tarefas.Find(id); 
            if(tarefa == null)
            {
                return NotFound();
            }

            _context.Remove(tarefa);
            return Ok(tarefa);
        }

        [HttpGet]

        public async Task<IActionResult> GetTodasTarefas()
        {
            var tarefas = await _context.Tarefas.ToListAsync();
            return Ok(tarefas);
        }
    }
}
