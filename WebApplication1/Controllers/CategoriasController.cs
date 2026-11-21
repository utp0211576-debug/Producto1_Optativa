using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controlador REST para la gestión CRUD de categorías de productos (<c>/api/categorias</c>).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa el controlador de categorías con el contexto de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/>.</param>
        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene el listado completo de categorías registradas.
        /// </summary>
        /// <returns>Lista de objetos <see cref="Categoria"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTodas()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return Ok(categorias);
        }

        /// <summary>
        /// Consulta una categoría específica por su identificador.
        /// </summary>
        /// <param name="id">Identificador único de la categoría.</param>
        /// <returns>La categoría encontrada o <c>404 Not Found</c>.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            return Ok(categoria);
        }

        /// <summary>
        /// Registra una nueva categoría en PostgreSQL.
        /// </summary>
        /// <param name="nuevaCategoria">Datos de la categoría a crear.</param>
        /// <returns>Respuesta <c>201 Created</c> con la categoría creada.</returns>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Categoria nuevaCategoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Categorias.Add(nuevaCategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId), new { id = nuevaCategoria.IdCategoria }, nuevaCategoria);
        }

        /// <summary>
        /// Actualiza el nombre y descripción de una categoría existente.
        /// </summary>
        /// <param name="id">ID de la categoría a modificar.</param>
        /// <param name="editada">Nuevos valores de la categoría.</param>
        /// <returns>Categoría actualizada o <c>404 Not Found</c>.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Categoria editada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cat = await _context.Categorias.FindAsync(id);
            if (cat == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            cat.Nombre = editada.Nombre;
            cat.Descripcion = editada.Descripcion;

            await _context.SaveChangesAsync();
            return Ok(cat);
        }

        /// <summary>
        /// Elimina una categoría por su identificador.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar.</param>
        /// <returns><c>204 No Content</c> si se eliminó correctamente.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cat = await _context.Categorias.FindAsync(id);
            if (cat == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            _context.Categorias.Remove(cat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}