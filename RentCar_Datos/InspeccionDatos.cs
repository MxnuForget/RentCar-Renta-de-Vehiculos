using System.Data;
public class InspeccionDatos
{
    public void Insertar(Inspeccion i)
    {
        string cols = "id_alquiler, tipo_inspeccion, fecha_inspeccion, observaciones";
        string vals = "@alq, @tipo, @fecha, @obs";
        string sql = Db.EsIdentity("Inspeccion", "id_inspeccion")
            ? "INSERT INTO Inspeccion (" + cols + ") VALUES (" + vals + ")"
            : "INSERT INTO Inspeccion (id_inspeccion, " + cols + ") SELECT ISNULL(MAX(id_inspeccion), 0) + 1, " + vals + " FROM Inspeccion";
        Db.Ejecutar(sql, Db.P("@alq", i.IdAlquiler), Db.P("@tipo", i.TipoInspeccion),
            Db.P("@fecha", i.FechaInspeccion), Db.P("@obs", i.Observaciones));
    }
    public void Modificar(Inspeccion i)
    {
        Db.Ejecutar(@"UPDATE Inspeccion SET id_alquiler=@alq, tipo_inspeccion=@tipo, fecha_inspeccion=@fecha, observaciones=@obs
                      WHERE id_inspeccion=@id",
            Db.P("@alq", i.IdAlquiler), Db.P("@tipo", i.TipoInspeccion), Db.P("@fecha", i.FechaInspeccion),
            Db.P("@obs", i.Observaciones), Db.P("@id", i.IdInspeccion));
    }
    public void Eliminar(int id)
    {
        Db.Ejecutar("DELETE FROM Inspeccion WHERE id_inspeccion=@id", Db.P("@id", id));
    }
    public DataTable Listar()
    {
        return Db.Consultar("SELECT id_inspeccion, id_alquiler, tipo_inspeccion, fecha_inspeccion, observaciones FROM Inspeccion ORDER BY id_inspeccion DESC");
    }
}
