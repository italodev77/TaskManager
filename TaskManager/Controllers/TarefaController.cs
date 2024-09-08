using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Context;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaContext _context;

        public TarefaController(TarefaContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> CriarTarefa(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                return BadRequest("A tarefa não pode ser nula.");
            }

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasTarefas()
        {
            var tarefas = await _context.Tarefas.ToListAsync();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo([FromQuery] string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
            {
                return BadRequest("O título é obrigatório.");
            }

            var tarefas = await _context.Tarefas
                .Where(t => t.Descricao.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus([FromQuery] EnumStatusTarefa status)
        {
            var tarefas = await _context.Tarefas
                .Where(x => x.Status == status)
                .ToListAsync();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData([FromQuery] DateTime data)
        {
            var tarefas = await _context.Tarefas
                .Where(x => x.Data.Date == data.Date)
                .ToListAsync();

            return Ok(tarefas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            if (tarefa == null)
            {
                return BadRequest("A tarefa não pode ser nula.");
            }

            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            var tarefaBanco = await _context.Tarefas.FindAsync(id);
            if (tarefaBanco == null)
            {
                return NotFound();
            }

            
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            await _context.SaveChangesAsync();

            return Ok(tarefaBanco);
        }
    }
}
