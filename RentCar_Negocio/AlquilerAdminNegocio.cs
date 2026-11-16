using System;
using System.Data;

public class AlquilerAdminNegocio
{
    private AlquilerAdminDatos datos = new AlquilerAdminDatos();

    public DataTable Filtrar(string estado, DateTime? desde, DateTime? hasta, string texto)
    {
        Permisos.RequerirAdministrador();
        if (desde.HasValue && hasta.HasValue && hasta.Value.Date < desde.Value.Date)
            throw new Exception("La fecha 'hasta' no puede ser anterior a 'desde'.");
        return datos.Filtrar(estado, desde, hasta, texto);
    }

    public void Finalizar(int id)
    {
        Permisos.RequerirAdministrador();
        if (id <= 0) throw new Exception("Seleccione un alquiler de la tabla.");
        if (datos.Finalizar(id) == 0) throw new Exception("Solo se pueden finalizar alquileres en estado Activo.");
    }

    public void Cancelar(int id)
    {
        Permisos.RequerirAdministrador();
        if (id <= 0) throw new Exception("Seleccione un alquiler de la tabla.");
        if (datos.Cancelar(id) == 0) throw new Exception("Solo se pueden cancelar alquileres en estado Activo.");
    }
}
