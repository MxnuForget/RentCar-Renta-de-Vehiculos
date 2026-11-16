using System;
using System.Data;

// Capa de negocio: validaciones antes de llamar a la capa de datos
public class MetodoPagoNegocio
{
    private MetodoPagoDatos datos = new MetodoPagoDatos();

    public DataTable Listar() { return datos.Listar(); }

    public void Modificar(MetodoPago m)
    {
        if (m.IdMetodoPago <= 0) throw new Exception("Seleccione un método de pago de la tabla.");

        m.NombreMetodo = (m.NombreMetodo ?? "").Trim();
        if (m.NombreMetodo == "") throw new Exception("El nombre del método de pago es obligatorio.");
        if (m.NombreMetodo.Length > 50) throw new Exception("El nombre no puede tener más de 50 caracteres.");
        if (datos.ExisteNombre(m.NombreMetodo, m.IdMetodoPago))
            throw new Exception("Ya existe un método de pago con ese nombre.");

        datos.Modificar(m);
    }

    public void Eliminar(int id)
    {
        if (id <= 0) throw new Exception("Seleccione un método de pago de la tabla.");
        if (datos.EstaEnUso(id))
            throw new Exception("No se puede eliminar: este método ya fue usado en uno o más pagos.");

        datos.Eliminar(id);
    }
}
