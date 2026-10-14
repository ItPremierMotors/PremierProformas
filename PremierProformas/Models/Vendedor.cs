namespace PremierProformas.Models;

public class Vendedor : EntidadBase
{
    public string NombreCompleto { get; set; } = "";
    public int SucursalId { get; set; }
    public Sucursal Sucursal { get; set; } = null!;
    public string? UsuarioId { get; set; }          // Id de AspNetUsers
}
