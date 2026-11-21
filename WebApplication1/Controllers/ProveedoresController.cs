using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controlador REST para la administración CRUD de proveedores (<c>/api/proveedores</c>).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa el controlador de proveedores con el contexto de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/>.</param>
        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista completa de proveedores registrados.
        /// </summary>
        /// <returns>Lista de objetos <see cref="Proveedor"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return Ok(proveedores);
        }

        /// <summary>
        /// Consulta un proveedor específico mediante su identificador.
        /// </summary>
        /// <param name="id">ID del proveedor.</param>
        /// <returns>El proveedor encontrado o <c>404 Not Found</c>.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return NotFound(new { mensaje = "Proveedor no encontrado." });

            return Ok(proveedor);
        }

        /// <summary>
        /// Registra un nuevo proveedor en la base de datos.
        /// </summary>
        /// <param name="nuevoProveedor">Datos del proveedor a registrar.</param>
        /// <returns>Respuesta <c>201 Created</c> con el proveedor creado.</returns>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Proveedor nuevoProveedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Proveedores.Add(nuevoProveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId), new { id = nuevoProveedor.IdProveedor }, nuevoProveedor);
        }

        /// <summary>
        /// Actualiza los datos de contacto y dirección de un proveedor existente.
        /// </summary>
        /// <param name="id">ID del proveedor a modificar.</param>
        /// <param name="editado">Nuevos datos del proveedor.</param>
        /// <returns>El proveedor actualizado o <c>404 Not Found</c>.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Proveedor editado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var prov = await _context.Proveedores.FindAsync(id);
            if (prov == null)
                return NotFound(new { mensaje = "Proveedor no encontrado." });

            prov.NombreEmpresa = editado.NombreEmpresa;
            prov.ContactoNombre = editado.ContactoNombre;
            prov.Telefono = editado.Telefono;
            prov.Email = editado.Email;
            prov.Direccion = editado.Direccion;

            await _context.SaveChangesAsync();
            return Ok(prov);
        }

        /// <summary>
        /// Elimina un proveedor del sistema por su identificador.
        /// </summary>
        /// <param name="id">ID del proveedor a eliminar.</param>
        /// <returns><c>204 No Content</c> si se eliminó correctamente.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var prov = await _context.Proveedores.FindAsync(id);
            if (prov == null)
                return NotFound(new { mensaje = "Proveedor no encontrado." });

            _context.Proveedores.Remove(prov);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}