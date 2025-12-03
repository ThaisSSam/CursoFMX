using GerenciadorTarefasApi.Infra.DTOs;
using GerenciadorTarefasApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefasApi.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefasController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

       
        [HttpGet]
        public ActionResult<List<TarefaDto>> ObterTodas()
        {
            var tarefas = _tarefaService.ObterTodos();
            return Ok(tarefas);
        }
        [HttpGet("{id}")]
        public ActionResult<TarefaDto> ObterPorId(int id)
        {
            var tarefa = _tarefaService.ObterPorId(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }
        [HttpPost]
        public ActionResult<TarefaDto> Adicionar([FromBody] CriarTarefaDto tarefaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TarefaDto tarefaAdicionada = _tarefaService.Adicionar(tarefaDto);

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefaAdicionada.Id }, tarefaAdicionada);
        }
        
        [HttpPut("{id}")]
        public ActionResult<TarefaDto> Atualizar(int id, [FromBody] AtualizarTarefaDto tarefaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var tarefaAtualizada = _tarefaService.Atualizar(id, tarefaDto);
            
            if (tarefaAtualizada == null)
            {
                return NotFound();
            }
            
            return Ok(tarefaAtualizada);
        }
        
        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            var sucesso = _tarefaService.Deletar(id);

            if (!sucesso)
            {
                return NotFound();
            }

            return Ok(new { mensagem = "Deletado com sucesso!" });
        }

        [HttpPatch("{id}/concluir")]
        public ActionResult MarcarComoConcluida(int id)
        {
            var sucesso = _tarefaService.MarcarComoConcluida(id);

            if (!sucesso)
            {
                return NotFound("Tarefa não encontrada ou já concluída.");
            }
            return Ok(new { mensagem = "Marcada como concluída." });
        }


        [HttpPost("{tarefaId}/tags/{tagId}")]
        public ActionResult AssociarTag(int tarefaId, int tagId)
        {
            var sucesso = _tarefaService.AssociarTag(tarefaId, tagId);

            if (!sucesso)
            {
                return NotFound("Tarefa ou Tag não encontrada.");
            }
            
            return NoContent(); 
        }
    }
}