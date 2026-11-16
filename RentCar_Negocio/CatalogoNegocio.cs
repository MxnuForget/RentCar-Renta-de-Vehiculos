using System.Data;
// Para llenar los ComboBox sin que Presentacion toque la capa Datos
public class CatalogoNegocio
{
    private CatalogoDatos c = new CatalogoDatos();
    public DataTable Clientes() { return c.Clientes(); }
    public DataTable Empleados() { return c.Empleados(); }
    public DataTable Vehiculos() { return c.Vehiculos(); }
    public DataTable MetodosPago() { return c.MetodosPago(); }
    public DataTable Alquileres() { return c.Alquileres(); }
    public DataTable Personas() { return c.Personas(); }
    public DataTable Cargos() { return c.Cargos(); }
    public DataTable Sucursales() { return c.Sucursales(); }
    public DataTable Inspecciones() { return c.Inspecciones(); }

    // Se leen de la restricción CHECK de la base; si no hay, se usan estos valores
    public string[] EstadosAlquiler()
    {
        return c.ValoresPermitidos("Alquiler", "estado_alquiler", new[] { "Activo", "Finalizado", "Cancelado" });
    }
    public string[] TiposInspeccion()
    {
        return c.ValoresPermitidos("Inspeccion", "tipo_inspeccion", new[] { "Entrega", "Devolucion" });
    }
}
