using Ferreteri.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ferreteri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosService _productosService;

        public ProductosController(IProductosService productosService)
        {
            _productosService = productosService;
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _productosService.GetCount();
            return Ok(count);
        }

        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarProductos()
        {
            var productos = await _productosService.ListarProductos();
            return Ok(productos);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddProducto([FromBody] Models.Producto producto)
        {
            try
            {
                var filasAfectadas = await _productosService.AddProducto(producto);
                return Ok(filasAfectadas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProducto([FromBody] Models.Producto producto)
        {
            try
            {
                var filasAfectadas = await _productosService.UpdateProducto(producto);
                if (filasAfectadas == 0)
                {
                    return NotFound(new { mensaje = "El producto que intenta actualizar no existe." });
                }
                return Ok(filasAfectadas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<IActionResult> RemoveProducto(int id)
        {
            try
            {
                var filasAfectadas = await _productosService.RemoveProducto(id);
                if (filasAfectadas == 0)
                {
                    return NotFound(new { mensaje = "El producto que intenta eliminar no existe." });
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
