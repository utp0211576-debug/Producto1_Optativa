using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    /// <summary>
    /// Contexto principal de Entity Framework Core que gestiona la conexión y el mapeo
    /// de entidades hacia la base de datos PostgreSQL alojada en Neon.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ApplicationDbContext"/> con las opciones configuradas.
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto (cadena de conexión, proveedor Npgsql, etc.).</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>Tabla de categorías de productos.</summary>
        public DbSet<Categoria> Categorias { get; set; }

        /// <summary>Tabla de proveedores registrados.</summary>
        public DbSet<Proveedor> Proveedores { get; set; }

        /// <summary>Tabla principal del catálogo de productos e inventario.</summary>
        public DbSet<Producto> Productos { get; set; }

        /// <summary>Tabla de auditoría y movimientos de entrada/salida de inventario.</summary>
        public DbSet<HistorialInventario> HistorialInventario { get; set; }

        /// <summary>Tabla de usuarios con acceso al sistema y sus roles.</summary>
        public DbSet<Usuario> Usuarios { get; set; }
    }
}