namespace PremierProformas.Models;

public class Sucursal : EntidadBase
{
    public string Codigo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Telefono { get; set; } = "";
    public int? JefeVentasId { get; set; }
    public Vendedor? JefeVentas { get; set; }
    public List<Vendedor> Vendedores { get; set; } = new();
}
