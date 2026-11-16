using System.Data;
// CRUD de la tabla Cliente (la persona ya debe existir en Persona)
public class ClienteDatos
{
    public void Insertar(Cliente c)
    {
        Db.Ejecutar("INSERT INTO Cliente (id_persona, licencia_conducir, fecha_venc_licencia) VALUES (@id, @lic, @venc)",
            Db.P("@id", c.IdPersona), Db.P("@lic", c.LicenciaConducir), Db.P("@venc", c.FechaVencLicencia));
    }
    public void Modificar(Cliente c)
    {
        Db.Ejecutar("UPDATE Cliente SET licencia_conducir=@lic, fecha_venc_licencia=@venc WHERE id_persona=@id",
            Db.P("@lic", c.LicenciaConducir), Db.P("@venc", c.FechaVencLicencia), Db.P("@id", c.IdPersona));
    }
    public void Eliminar(int id)
    {
        Db.Ejecutar("DELETE FROM Cliente WHERE id_persona=@id", Db.P("@id", id));
    }
    public DataTable Listar()
    {
        return Db.Consultar(@"SELECT c.id_persona,
                                     p.nombres + ' ' + p.ap_paterno AS cliente, p.ci,
                                     c.licencia_conducir, c.fecha_venc_licencia
                              FROM Cliente c JOIN Persona p ON p.id_persona = c.id_persona
                              ORDER BY c.id_persona DESC");
    }
}
