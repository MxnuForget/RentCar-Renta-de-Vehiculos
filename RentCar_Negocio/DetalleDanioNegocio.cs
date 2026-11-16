using System;
using System.Data;
public class DetalleDanioNegocio
{
    private DetalleDanioDatos datos = new DetalleDanioDatos();
    private void Validar(DetalleDanio d)
    {
        if (d.IdInspeccion <= 0) throw new Exception("Seleccione una inspección.");
        if (string.IsNullOrWhiteSpace(d.DescripcionDanio)) throw new Exception("La descripción es obligatoria.");
        if (d.CostoReparacion < 0) throw new Exception("El costo no puede ser negativo.");
    }
    public void Registrar(DetalleDanio d) { Validar(d); datos.Insertar(d); }
    public void Modificar(DetalleDanio d)
    {
        if (d.IdInspeccion <= 0 || d.NumItem <= 0) throw new Exception("Seleccione un daño de la tabla.");
        Validar(d); datos.Modificar(d);
    }
    public void Eliminar(int idInspeccion, int numItem)
    {
        if (idInspeccion <= 0 || numItem <= 0) throw new Exception("Seleccione un daño de la tabla.");
        datos.Eliminar(idInspeccion, numItem);
    }
    public DataTable Listar() { return datos.Listar(); }
}
