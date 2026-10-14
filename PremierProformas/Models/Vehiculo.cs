namespace PremierProformas.Models;

public class Vehiculo : EntidadBase
{
    public string CodigoInventario { get; set; } = "";
    public int MarcaId { get; set; }
    public Marca Marca { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string Modelo { get; set; } = "";
    public string? ModeloComercial { get; set; }
    public CondicionVehiculo Condicion { get; set; } = CondicionVehiculo.Nuevo;
    public string? Tipo { get; set; }
    public string? Transmision { get; set; }
    public int? CapacidadPasajeros { get; set; }
    public int? Puertas { get; set; }
    public string? Color { get; set; }
    public int Anio { get; set; }
    public string Vin { get; set; } = "";
    public string? Chasis { get; set; }
    public string? Motor { get; set; }
    public string? Combustible { get; set; }
    public string? Cilindraje { get; set; }
    public string? Garantia { get; set; }
    public string? Ubicacion { get; set; }
    public string? PolizaImportacion { get; set; }
    public string? NumeroDuca { get; set; }
    public string? FacturaExterna { get; set; }
    public DateOnly? FechaIngreso { get; set; }
    public EstatusVehiculo Estatus { get; set; } = EstatusVehiculo.Disponible;
    public string? Comentarios { get; set; }
}
