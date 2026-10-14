namespace PremierProformas.Models;

public enum CondicionVehiculo
{
    Nuevo,
    Usado
}

public enum EstatusVehiculo
{
    Disponible,
    Reservado,
    Vendido,
    EnTransito,
    Baja
}

public enum EstadoProforma
{
    Borrador,
    Emitida,
    Aprobada,
    Vencida,
    Anulada
}
