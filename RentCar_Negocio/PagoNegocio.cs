using System;
using System.Data;
public class PagoNegocio
{
    private PagoDatos datos = new PagoDatos();
    private void Validar(Pago p)
    {
        if (p.IdAlquiler <= 0) throw new Exception("Seleccione un alquiler.");
        if (p.IdMetodoPago <= 0) throw new Exception("Seleccione un método de pago.");
        if (p.Monto <= 0) throw new Exception("El monto debe ser mayor a 0.");
    }
    public void Registrar(Pago p) { Validar(p); datos.Insertar(p); }
    public void Modificar(Pago p)
    {
        if (p.IdAlquiler <= 0 || p.NumPago <= 0) throw new Exception("Seleccione un pago de la tabla.");
        Validar(p); datos.Modificar(p);
    }
    public void Eliminar(int idAlquiler, int numPago)
    {
        if (idAlquiler <= 0 || numPago <= 0) throw new Exception("Seleccione un pago de la tabla.");
        datos.Eliminar(idAlquiler, numPago);
    }
    public DataTable Listar() { return datos.Listar(); }
}
