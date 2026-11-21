using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controlador REST para consultar el historial y registrar movimientos de inventario
    /// (<c>ENTRADA</c>, <c>SALIDA</c>, <c>AJUSTE</c>), delegando el recálculo de stock
    /// a un Trigger en PostgreSQL (Neon).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa el controlador de inventario con el contexto de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/>.</param>
        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene el historial completo de movimientos ordenado del más reciente al más antiguo.
        /// </summary>
        /// <returns>Lista ordenada de registros <see cref="HistorialInventario"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetHistorial()
        {
            var historial = await _context.HistorialInventario
                .OrderByDescending(h => h.FechaMovimiento)
                .ToListAsync();

            return Ok(historial);
        }

        /// <summary>
        /// Registra un movimiento de inventario y dispara automáticamente el Trigger en Neon
        /// para actualizar las existencias del producto.
        /// </summary>
        /// <param name="movimiento">Datos del movimiento (<c>ENTRADA</c>, <c>SALIDA</c> o <c>AJUSTE</c>).</param>
        /// <returns>
        /// <c>200 OK</c> si se procesó correctamente, o <c>400 Bad Request</c> si el trigger
        /// rechaza la operación (por ejemplo, por stock insuficiente).
        /// </returns>
        [HttpPost("movimiento")]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] HistorialInventario movimiento)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.HistorialInventario.Add(movimiento);
                await _context.SaveChangesAsync(); // PostgreSQL dispara el Trigger automáticamente aquí

                return Ok(new { mensaje = "Movimiento registrado con éxito. El stock fue actualizado por el Trigger en Neon." });
            }
            catch (DbUpdateException ex)
            {
                // Captura excepciones lanzadas por el Trigger de Neon (ej. stock insuficiente)
                return BadRequest(new
                {
                    mensaje = "Error al procesar movimiento en la base de datos.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}