using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

// Listas para llenar ComboBox (columnas Id y Nombre)
public class CatalogoDatos
{
    public DataTable Clientes() { return Db.Consultar("SELECT c.id_persona AS Id, p.nombres + ' ' + p.ap_paterno AS Nombre FROM Cliente c JOIN Persona p ON p.id_persona = c.id_persona ORDER BY p.nombres"); }
    public DataTable Empleados() { return Db.Consultar("SELECT e.id_persona AS Id, p.nombres + ' ' + p.ap_paterno AS Nombre FROM Empleado e JOIN Persona p ON p.id_persona = e.id_persona WHERE e.usuario <> 'reservas_web' ORDER BY p.nombres"); }
    public DataTable Vehiculos() { return Db.Consultar("SELECT id_vehiculo AS Id, placa + ' - ' + nombre_modelo AS Nombre FROM Vehiculo ORDER BY placa"); }
    public DataTable MetodosPago() { return Db.Consultar("SELECT id_metodo_pago AS Id, nombre_metodo AS Nombre FROM MetodoPago ORDER BY nombre_metodo"); }
    public DataTable Cargos() { return Db.Consultar("SELECT id_cargo AS Id, nombre_cargo AS Nombre FROM Cargo ORDER BY nombre_cargo"); }
    public DataTable Sucursales() { return Db.Consultar("SELECT id_sucursal AS Id, nombre_sucursal AS Nombre FROM Sucursal ORDER BY nombre_sucursal"); }
    public DataTable Personas() { return Db.Consultar("SELECT id_persona AS Id, nombres + ' ' + ap_paterno + ' (CI ' + ci + ')' AS Nombre FROM Persona WHERE ci <> '0000000' ORDER BY nombres"); }
    public DataTable Alquileres() { return Db.Consultar("SELECT id_alquiler AS Id, 'Alquiler #' + CAST(id_alquiler AS VARCHAR(10)) AS Nombre FROM Alquiler ORDER BY id_alquiler DESC"); }
    public DataTable Inspecciones() { return Db.Consultar("SELECT id_inspeccion AS Id, 'Inspección #' + CAST(id_inspeccion AS VARCHAR(10)) AS Nombre FROM Inspeccion ORDER BY id_inspeccion DESC"); }

    // Lee los valores permitidos de una columna con restricción CHECK (por ejemplo estado_alquiler).
    // Si no hay CHECK, devuelve los valores por defecto.
    public string[] ValoresPermitidos(string tabla, string columna, string[] porDefecto)
    {
        try
        {
            DataTable dt = Db.Consultar(
                "SELECT definition FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(@t) AND definition LIKE '%' + @c + '%'",
                Db.P("@t", "dbo." + tabla), Db.P("@c", columna));
            List<string> vals = new List<string>();
            foreach (DataRow r in dt.Rows)
                foreach (Match m in Regex.Matches(Convert.ToString(r["definition"]), "'([^']*)'"))
                    if (!vals.Contains(m.Groups[1].Value)) vals.Add(m.Groups[1].Value);
            if (vals.Count > 0) { vals.Sort(); return vals.ToArray(); }
        }
        catch { }
        return porDefecto;
    }
}
