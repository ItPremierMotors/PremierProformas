namespace PremierProformas.Models;

public class Marca : EntidadBase
{
    public string Nombre { get; set; } = "";
    public string Prefijo { get; set; } = "";
    public string? LogoArchivo { get; set; }
    public List<Vehiculo> Vehiculos { get; set; } = new();
}
