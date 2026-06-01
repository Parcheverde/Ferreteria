using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ferreteri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly Services.ICategoriasService _categoriasService;
        private readonly Models.FerreteriContext _context;
        
        public CategoriasController(Services.ICategoriasService categoriasService, Models.FerreteriContext context)
        {
            _categoriasService = categoriasService;
            _context = context;

        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _categoriasService.GetCount();
            return Ok(count);
        }

        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarCategorias()
        {
            var categorias = await _categoriasService.ListarCategorias();
            return Ok(categorias);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCategoria([FromBody] Models.Categoria categoria)
        {
            if (categoria == null || string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre de la categoría es obligatorio y no puede estar vacío." });
            }

            // Evitar que registros duplicados que confundan el inventario
            var existeDuplicado = await _context.Categorias.AnyAsync(c => c.Nombre.ToLower() == categoria.Nombre.ToLower());
            if (existeDuplicado)
            {
                return BadRequest(new { mensaje = $"Operación rechazada. Ya existe una categoría llamada '{categoria.Nombre}'." });
            }

            var filasAfectadas = await _categoriasService.AddCategoria(categoria);
            return Ok(filasAfectadas);
        }

        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<IActionResult> RemoveCategoria(int id)
        {
            try
            {
                var tieneProductosAsociados = await _context.Productos.AnyAsync(p => p.FkIdCategoria == id);
                if (tieneProductosAsociados)
                {
                    return BadRequest(new { mensaje = "No se puede eliminar la categoría porque existen productos de la ferretería asociados a ella." });
                }

                var filasAfectadas = await _categoriasService.RemoveCategoria(id);

                if (filasAfectadas == 0)
                {
                    return NotFound(new { mensaje = "La categoría que intenta eliminar no existe." });
                }

                return Ok(filasAfectadas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
