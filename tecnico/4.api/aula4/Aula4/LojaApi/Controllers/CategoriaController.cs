using System;
using LojaApi.Entities;
using LojaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriasController(ICategoriaService categoriaService) { _categoriaService = categoriaService; }

        [HttpGet]
        public ActionResult<List<Categoria>> GetAll()
        {
            return Ok(_categoriaService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _categoriaService.ObterPorId(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }
    }