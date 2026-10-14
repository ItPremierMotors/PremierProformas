namespace PremierProformas.Models;

/// <summary>
/// Campos comunes de auditoría y borrado lógico.
/// El borrado lógico se maneja manualmente: filtrar por Activo en cada consulta.
/// </summary>
public abstract class EntidadBase
{
    public int Id { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public string? CreadoPor { get; set; }
    public DateTime? ModificadoEn { get; set; }
    public string? ModificadoPor { get; set; }
}
