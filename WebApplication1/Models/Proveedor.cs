using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// Representa a un proveedor o empresa distribuidora vinculada al inventario.
    /// </summary>
    [Table("proveedores")]
    public class Proveedor
    {
        /// <summary>Identificador único del proveedor (Clave primaria).</summary>
        [Key]
        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        /// <summary>Razón social o nombre comercial de la empresa proveedora.</summary>
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(150)]
        [Column("nombre_empresa")]
        public string NombreEmpresa { get; set; } = string.Empty;

        /// <summary>Nombre de la persona de contacto directo.</summary>
        [Column("contacto_nombre")]
        public string? ContactoNombre { get; set; }

        /// <summary>Número telefónico de contacto.</summary>
        [Column("telefono")]
        public string? Telefono { get; set; }

        /// <summary>Correo electrónico de contacto validado.</summary>
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>Dirección física o fiscal del proveedor.</summary>
        [Column("direccion")]
        public string? Direccion { get; set; }
    }
}