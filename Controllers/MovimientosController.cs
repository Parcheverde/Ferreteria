using Ferreteri.Models;
using Ferreteri.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ferreteri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientosService _movimientosService;

        public MovimientosController(IMovimientosService movimientosService)
        {
            _movimientosService = movimientosService;
        }

        // GET: api/Movimientos
        [HttpGet]
        public async Task<ActionResult<List<Movimiento>>> GetAll()
        {
            var movimientos = await _movimientosService.GetAll();
            return Ok(movimientos);
        }

        // GET: api/Movimientos/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            var total = await _movimientosService.GetCount();
            return Ok(total);
        }

        // GET: api/Movimientos/tipo/entrada
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<List<Movimiento>>> GetByTipo(string tipo)
        {
            var movimientos = await _movimientosService.GetByTipo(tipo);

            if (!movimientos.Any())
                return NotFound($"No se encontraron movimientos de tipo '{tipo}'.");

            return Ok(movimientos);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovimiento([FromBody] Movimiento movimiento)
        {
            try
            {
                var filasAfectadas = await _movimientosService.AddMovimiento(movimiento);
                return Ok(filasAfectadas);
            }
            catch (Exception ex)
            {
                // Si no hay stock suficiente, capturará el error del service y enviará el mensaje controlado
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}