using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            var objTarefa = _context.Tarefas.Find(id);
            if (objTarefa == null)
                return NoContent(); // Notfound faz parte do range servidor, neste caso a consulta ocorreu com sucesso, porem nao encontrou o pesquisado.

            return Ok(objTarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            return Ok(_context.Tarefas.ToList());
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            if(string.IsNullOrEmpty(titulo))
                return BadRequest(new { Erro = "O título da tarefa não pode ser vazio" });

            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));
            
            if (tarefa == null)
                return NoContent();

            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            var objTarefas = _context.Tarefas.Where(x => x.Status == status);
            if (objTarefas == null)
                return NoContent();

            return Ok(objTarefas);            
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Tarefas.Add(tarefa);
            var resultado = _context.SaveChanges();
            
            if (resultado == 0)
                return BadRequest(new { Erro = "Erro ao criar a tarefa" });
            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NoContent();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            _context.Tarefas.Update(tarefaBanco);
            var resultado = _context.SaveChanges();

            if (resultado == 0)
                return BadRequest(new { Erro = "Erro ao atualizar a tarefa" });

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NoContent();

            _context.Tarefas.Remove(tarefaBanco);
            var resultado = _context.SaveChanges();
            
            if (resultado == 0)
                return BadRequest(new { Erro = "Erro ao deletar a tarefa" });

            return Ok();
        }
    }
}
