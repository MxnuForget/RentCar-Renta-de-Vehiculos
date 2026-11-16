using System;
using System.Data;
public class AlquilerNegocio
{
    private AlquilerDatos datos = new AlquilerDatos();
    private void Validar(Alquiler a)
    {
        if (a.IdCliente <= 0) throw new Exception("Seleccione un cliente.");
        if (a.IdEmpleado <= 0) throw new Exception("Seleccione un empleado.");
        if (a.IdVehiculo <= 0) throw new Exception("Seleccione un vehículo.");
        if (string.IsNullOrWhiteSpace(a.EstadoAlquiler)) throw new Exception("Seleccione el estado del alquiler.");
        if (a.FechaFinPactada.Date < a.FechaInicio.Date)
            throw new Exception("La fecha de fin no puede ser anterior a la de inicio.");
        if (a.FechaDevolucionReal.HasValue && a.FechaDevolucionReal.Value.Date < a.FechaInicio.Date)
            throw new Exception("La devolución no puede ser anterior a la fecha de inicio.");
    }
    public void Registrar(Alquiler a) { Validar(a); datos.Insertar(a); }
    public void Modificar(Alquiler a)
    {
        if (a.IdAlquiler <= 0) throw new Exception("Seleccione un alquiler de la tabla.");
        Validar(a); datos.Modificar(a);
    }
    public void Eliminar(int id)
    {
        if (id <= 0) throw new Exception("Seleccione un alquiler de la tabla.");
        datos.Eliminar(id);
    }
    public DataTable Listar() { return datos.Listar(); }
}
