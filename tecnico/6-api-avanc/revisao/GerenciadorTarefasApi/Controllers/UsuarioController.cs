using GerenciadorTarefasApi.Infra.DTOs;
using GerenciadorTarefasApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GerenciadorTarefasApi.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<List<UsuarioDto>> ObterTodos()
        {
            var usuarios = _usuarioService.ObterTodos();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public ActionResult<UsuarioDto> ObterPorId(int id)
        {
            var usuario = _usuarioService.ObterPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }


        [HttpPost]
        public ActionResult<UsuarioDto> Adicionar([FromBody] CriarUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioAdicionado = _usuarioService.Adicionar(usuarioDto);

            return CreatedAtAction(nameof(ObterPorId), new { id = usuarioAdicionado.Id }, usuarioAdicionado);
        }

        [HttpPut("{id}")]
        public ActionResult<UsuarioDto> Atualizar(int id, [FromBody] CriarUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var usuarioAtualizado = _usuarioService.Atualizar(id, usuarioDto);
            
            if (usuarioAtualizado == null)
            {
                return NotFound();
            }
            
            return Ok(usuarioAtualizado);
        }
        
        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            var sucesso = _usuarioService.Deletar(id);

            if (!sucesso)
            {
                return NotFound();
            }

            return Ok(new { mensagem = "Deletado com sucesso!" }); 
        }
    }
}