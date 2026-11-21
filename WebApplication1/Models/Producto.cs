using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// Representa un artículo dentro del catálogo de inventario, incluyendo precios,
    /// niveles de stock y relaciones con categoría y proveedor.
    /// </summary>
    [Table("productos")]
    public class Producto
    {
        /// <summary>Identificador único del producto (Clave primaria).</summary>
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        /// <summary>Código de barras o SKU opcional del producto.</summary>
        [Column("codigo_barras")]
        public string? CodigoBarras { get; set; }

        /// <summary>Nombre comercial del producto (obligatorio, máximo 150 caracteres).</summary>
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Descripción o especificaciones adicionales del artículo.</summary>
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        /// <summary>Clave foránea opcional hacia la categoría asignada.</summary>
        [Column("id_categoria")]
        public int? IdCategoria { get; set; }

        /// <summary>Clave foránea opcional hacia el proveedor que surte el producto.</summary>
        [Column("id_proveedor")]
        public int? IdProveedor { get; set; }

        /// <summary>Costo de adquisición unitario del producto.</summary>
        [Range(0, 9999999.99, ErrorMessage = "El precio de compra debe ser un número positivo.")]
        [Column("precio_compra")]
        public decimal PrecioCompra { get; set; }

        /// <summary>Precio unitario de venta al público.</summary>
        [Range(0, 9999999.99, ErrorMessage = "El precio de venta debe ser un número positivo.")]
        [Column("precio_venta")]
        public decimal PrecioVenta { get; set; }

        /// <summary>Cantidad de unidades disponibles actualmente en almacén.</summary>
        [Column("stock_actual")]
        public int StockActual { get; set; }

        /// <summary>Umbral mínimo de existencias antes de generar alerta de reabastecimiento.</summary>
        [Range(0, 10000, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0.")]
        [Column("stock_minimo")]
        public int StockMinimo { get; set; } = 5;

        /// <summary>Fecha y hora UTC de alta del producto en el sistema.</summary>
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>Propiedad de navegación hacia la categoría asociada.</summary>
        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }

        /// <summary>Propiedad de navegación hacia el proveedor asociado.</summary>
        [ForeignKey("IdProveedor")]
        public Proveedor? Proveedor { get; set; }
    }
}