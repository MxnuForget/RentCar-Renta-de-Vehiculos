using System;
public class Pago
{
    public int IdAlquiler { get; set; }
    public int NumPago { get; set; }       // lo genera la capa de datos
    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public int IdMetodoPago { get; set; }
}
