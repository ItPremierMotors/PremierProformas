using System.ComponentModel.DataAnnotations;

namespace PremierProformas.ViewModels;

public class UsuarioListItemViewModel
{
    public string Id { get; set; } = "";
    public string Email { get; set; } = "";
    public string Roles { get; set; } = "";
    public string? Vendedor { get; set; }
    public string? Sucursal { get; set; }
    public bool Activo { get; set; }
}

public class CrearUsuarioViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Escribe un correo válido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Selecciona un rol.")]
    public string Role { get; set; } = "";

    public int? VendedorId { get; set; }

    [StringLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string? NombreVendedor { get; set; }

    public int? SucursalId { get; set; }
}