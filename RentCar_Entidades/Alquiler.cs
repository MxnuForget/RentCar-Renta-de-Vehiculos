using System;
public class Alquiler
{
    public int IdAlquiler { get; set; }
    public int IdCliente { get; set; }
    public int IdEmpleado { get; set; }
    public int IdVehiculo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFinPactada { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }   // null mientras no se devuelva
    public string EstadoAlquiler { get; set; }
}
