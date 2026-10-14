namespace PremierProformas.Models;

public class Cliente : EntidadBase
{
    public string NombreCompleto { get; set; } = "";
    public string Dni { get; set; } = "";
    public string? Rtn { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
}
