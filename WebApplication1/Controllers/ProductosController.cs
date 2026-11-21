using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controlador REST para la gestión del catálogo de productos y alertas de stock bajo
    /// (<c>/api/productos</c>).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa el controlador de productos con el contexto de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/>.</param>
        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los productos incluyendo los nombres de su categoría y proveedor asociados.
        /// </summary>
        /// <returns>Listado proyectado de productos.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Select(p => new
                {
                    p.IdProducto,
                    p.CodigoBarras,
                    p.Nombre,
                    p.Descripcion,
                    p.PrecioCompra,
                    p.PrecioVenta,
                    p.StockActual,
                    p.StockMinimo,
                    Categoria = p.Categoria != null ? p.Categoria.Nombre : "Sin categoría",
                    Proveedor = p.Proveedor != null ? p.Proveedor.NombreEmpresa : "Sin proveedor"
                })
                .ToListAsync();

            return Ok(productos);
        }

        /// <summary>
        /// Consulta un producto específico por su identificador único.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>El producto encontrado o <c>404 Not Found</c>.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });

            return Ok(producto);
        }

        /// <summary>
        /// Filtra y devuelve los productos cuyo stock actual es menor o igual a su stock mínimo.
        /// </summary>
        /// <returns>Lista de productos en nivel crítico de existencias.</returns>
        [HttpGet("stock-bajo")]
        public async Task<IActionResult> GetStockBajo()
        {
            var productosBajos = await _context.Productos
                .Where(p => p.StockActual <= p.StockMinimo)
                .ToListAsync();

            return Ok(productosBajos);
        }

        /// <summary>
        /// Registra un nuevo producto en el catálogo.
        /// </summary>
        /// <param name="nuevoProducto">Datos del producto a registrar.</param>
        /// <returns>Respuesta <c>201 Created</c> con el producto creado.</returns>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Producto nuevoProducto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId), new { id = nuevoProducto.IdProducto }, nuevoProducto);
        }

        /// <summary>
        /// Actualiza los datos comerciales y de umbral mínimo de un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="editado">Datos modificados del producto.</param>
        /// <returns>El producto actualizado o <c>404 Not Found</c>.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Producto editado)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound(new { mensaje = "Producto no encontrado." });

            prod.CodigoBarras = editado.CodigoBarras;
            prod.Nombre = editado.Nombre;
            prod.Descripcion = editado.Descripcion;
            prod.IdCategoria = editado.IdCategoria;
            prod.IdProveedor = editado.IdProveedor;
            prod.PrecioCompra = editado.PrecioCompra;
            prod.PrecioVenta = editado.PrecioVenta;
            prod.StockMinimo = editado.StockMinimo;

            await _context.SaveChangesAsync();
            return Ok(prod);
        }

        /// <summary>
        /// Elimina un producto del catálogo por su identificador.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns><c>204 No Content</c> si se eliminó correctamente.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound(new { mensaje = "Producto no encontrado." });

            _context.Productos.Remove(prod);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}