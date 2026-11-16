using System;
using System.Data;

// Capa de negocio de los REPORTES: revisa permisos y valida los filtros antes de llamar a la capa de datos
public class ReporteNegocio
{
    private ReporteDatos datos = new ReporteDatos();

    private static void ValidarFechas(DateTime ini, DateTime fin)
    {
        if (ini.Date > fin.Date)
            throw new Exception("La fecha inicial no puede ser mayor que la fecha final.");
    }

    // "Todos" (o vacio) = sin filtro -> null
    private static string Estado(string e)
    {
        return string.IsNullOrWhiteSpace(e) || e == "Todos" ? null : e;
    }

    // Reporte 1
    public DataTable Alquileres(DateTime ini, DateTime fin, string estado)
    {
        Permisos.RequerirEmpleado();
        ValidarFechas(ini, fin);
        return datos.Alquileres(ini, fin, Estado(estado));
    }

    public DataTable ResumenAlquileres(DateTime ini, DateTime fin, string estado)
    {
        Permisos.RequerirEmpleado();
        ValidarFechas(ini, fin);
        return datos.ResumenAlquileres(ini, fin, Estado(estado));
    }

    // Reporte 2
    public DataTable Pagos(DateTime ini, DateTime fin, int idMetodo)
    {
        Permisos.RequerirEmpleado();
        ValidarFechas(ini, fin);
        return datos.Pagos(ini, fin, idMetodo);
    }

    public DataTable ResumenPagos(DateTime ini, DateTime fin, int idMetodo)
    {
        Permisos.RequerirEmpleado();
        ValidarFechas(ini, fin);
        return datos.ResumenPagos(ini, fin, idMetodo);
    }

    // Reporte 3
    public DataTable MasAlquilados(DateTime ini, DateTime fin, int idCategoria)
    {
        Permisos.RequerirEmpleado();
        ValidarFechas(ini, fin);
        return datos.MasAlquilados(ini, fin, idCategoria);
    }

    // Reporte 4
    public DataTable FlotaPorCategoria(int idSucursal)
    {
        Permisos.RequerirEmpleado();
        return datos.FlotaPorCategoria(idSucursal);
    }

    // Reporte 5
    public DataTable ClientesFrecuentes(int minimo)
    {
        Permisos.RequerirEmpleado();
        if (minimo < 0) throw new Exception("La cantidad mínima de alquileres no puede ser negativa.");
        return datos.ClientesFrecuentes(minimo);
    }

    // Para llenar el ComboBox de categorias
    public DataTable Categorias()
    {
        Permisos.RequerirEmpleado();
        return datos.Categorias();
    }
}
