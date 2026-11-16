using System;
using System.Data;
public class InspeccionNegocio
{
    private InspeccionDatos datos = new InspeccionDatos();
    private void Validar(Inspeccion i)
    {
        if (i.IdAlquiler <= 0) throw new Exception("Seleccione un alquiler.");
        if (string.IsNullOrWhiteSpace(i.TipoInspeccion)) throw new Exception("Seleccione el tipo de inspección.");
    }
    public void Registrar(Inspeccion i) { Validar(i); datos.Insertar(i); }
    public void Modificar(Inspeccion i)
    {
        if (i.IdInspeccion <= 0) throw new Exception("Seleccione una inspección de la tabla.");
        Validar(i); datos.Modificar(i);
    }
    public void Eliminar(int id)
    {
        if (id <= 0) throw new Exception("Seleccione una inspección de la tabla.");
        datos.Eliminar(id);
    }
    public DataTable Listar() { return datos.Listar(); }
}
