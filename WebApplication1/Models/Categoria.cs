using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// Representa una categoría para clasificar los productos del inventario.
    /// Mapeada a la tabla <c>categorias</c> en PostgreSQL.
    /// </summary>
    [Table("categorias")]
    public class Categoria
    {
        /// <summary>Identificador único de la categoría (Clave primaria).</summary>
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        /// <summary>Nombre descriptivo de la categoría (máximo 100 caracteres).</summary>
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Descripción opcional con detalles sobre la categoría.</summary>
        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}