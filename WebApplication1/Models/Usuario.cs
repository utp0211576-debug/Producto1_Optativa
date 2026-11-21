using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// Representa una cuenta de usuario autorizada para acceder al sistema de inventario.
    /// </summary>
    [Table("usuarios")]
    public class Usuario
    {
        /// <summary>Identificador único del usuario (Clave primaria).</summary>
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        /// <summary>Nombre de usuario único utilizado para iniciar sesión.</summary>
        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>Contraseña encriptada (hash) almacenada en la base de datos.</summary>
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>Nombre completo del titular de la cuenta.</summary>
        [Column("nombre_completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>Rol asignado dentro del sistema (<c>Administrador</c>, <c>Vendedor</c>, <c>Almacenista</c>).</summary>
        [Column("rol")]
        public string Rol { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO (Data Transfer Object) empleado para recibir las credenciales en el inicio de sesión.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>Nombre de usuario ingresado.</summary>
        public string Usuario { get; set; } = string.Empty;

        /// <summary>Contraseña en texto plano enviada en la petición de autenticación.</summary>
        public string Password { get; set; } = string.Empty;
    }
}