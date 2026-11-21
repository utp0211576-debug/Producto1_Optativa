using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// Registra cada movimiento histórico de existencias (entradas, salidas o ajustes)
    /// realizado sobre un producto en el inventario.
    /// </summary>
    [Table("historial_inventario")]
    public class HistorialInventario
    {
        /// <summary>Identificador único del movimiento (Clave primaria).</summary>
        [Key]
        [Column("id_movimiento")]
        public int IdMovimiento { get; set; }

        /// <summary>Identificador del producto afectado por el movimiento.</summary>
        [Required]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        /// <summary>Clasificación del movimiento: <c>ENTRADA</c>, <c>SALIDA</c> o <c>AJUSTE</c>.</summary>
        [Required(ErrorMessage = "El tipo de movimiento (ENTRADA, SALIDA, AJUSTE) es obligatorio.")]
        [Column("tipo_movimiento")]
        public string TipoMovimiento { get; set; } = string.Empty;

        /// <summary>Cantidad de unidades involucradas en la operación (debe ser mayor a 0).</summary>
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        [Column("cantidad")]
        public int Cantidad { get; set; }

        /// <summary>Fecha y hora en formato UTC en la que se registró el movimiento.</summary>
        [Column("fecha_movimiento")]
        public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow;

        /// <summary>Observación o justificación opcional del movimiento.</summary>
        [Column("motivo")]
        public string? Motivo { get; set; }
    }
}