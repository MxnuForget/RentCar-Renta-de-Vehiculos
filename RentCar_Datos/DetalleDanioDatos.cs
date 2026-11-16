using System.Data;
public class DetalleDanioDatos
{
    public void Insertar(DetalleDanio d)
    {
        Db.Ejecutar(@"INSERT INTO DetalleDanio (id_inspeccion, num_item, descripcion_danio, costo_reparacion)
                      SELECT @insp, ISNULL(MAX(num_item), 0) + 1, @desc, @costo
                      FROM DetalleDanio WHERE id_inspeccion = @insp",
            Db.P("@insp", d.IdInspeccion), Db.P("@desc", d.DescripcionDanio), Db.P("@costo", d.CostoReparacion));
    }
    public void Modificar(DetalleDanio d)
    {
        Db.Ejecutar("UPDATE DetalleDanio SET descripcion_danio=@desc, costo_reparacion=@costo WHERE id_inspeccion=@insp AND num_item=@num",
            Db.P("@desc", d.DescripcionDanio), Db.P("@costo", d.CostoReparacion), Db.P("@insp", d.IdInspeccion), Db.P("@num", d.NumItem));
    }
    public void Eliminar(int idInspeccion, int numItem)
    {
        Db.Ejecutar("DELETE FROM DetalleDanio WHERE id_inspeccion=@insp AND num_item=@num",
            Db.P("@insp", idInspeccion), Db.P("@num", numItem));
    }
    public DataTable Listar()
    {
        return Db.Consultar("SELECT id_inspeccion, num_item, descripcion_danio, costo_reparacion FROM DetalleDanio ORDER BY id_inspeccion DESC, num_item");
    }
}
