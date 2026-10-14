namespace PremierProformas.Models;

public class Proforma : EntidadBase
{
    public string Numero { get; set; } = "";
    public int SucursalId { get; set; }
    public Sucursal Sucursal { get; set; } = null!;
    public DateOnly FechaEmision { get; set; }
    public int DiasVigencia { get; set; } = 15;
    public string DirigidoA { get; set; } = "A quien interese";

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public int VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; } = null!;
    public int VendedorId { get; set; }
    public Vendedor Vendedor { get; set; } = null!;
    public string JefeVentasNombre { get; set; } = "";

    public decimal PrecioUsd { get; set; }
    public decimal TasaCambio { get; set; }
    public decimal PrecioConIsv { get; set; }
    public decimal PrecioSinIsv { get; set; }
    public decimal Isv { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal Prima { get; set; }
    public decimal SaldoFinanciar { get; set; }

    public EstadoProforma Estado { get; set; } = EstadoProforma.Emitida;
}
