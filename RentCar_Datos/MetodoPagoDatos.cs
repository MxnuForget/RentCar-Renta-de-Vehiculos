using System;
using System.Data;

// Capa de datos: aqui vive el SQL (siempre parametrizado)
public class MetodoPagoDatos
{
    public DataTable Listar()
    {
        return Db.Consultar("SELECT id_metodo_pago, nombre_metodo FROM MetodoPago ORDER BY id_metodo_pago");
    }

    public void Modificar(MetodoPago m)
    {
        Db.Ejecutar("UPDATE MetodoPago SET nombre_metodo=@nombre WHERE id_metodo_pago=@id",
            Db.P("@nombre", m.NombreMetodo), Db.P("@id", m.IdMetodoPago));
    }

    public void Eliminar(int id)
    {
        Db.Ejecutar("DELETE FROM MetodoPago WHERE id_metodo_pago=@id", Db.P("@id", id));
    }

    // true si ya existe OTRO metodo con ese nombre (la columna es UNIQUE)
    public bool ExisteNombre(string nombre, int idExcluir)
    {
        DataTable dt = Db.Consultar(
            "SELECT COUNT(*) FROM MetodoPago WHERE nombre_metodo=@nombre AND id_metodo_pago<>@id",
            Db.P("@nombre", nombre), Db.P("@id", idExcluir));
        return Convert.ToInt32(dt.Rows[0][0]) > 0;
    }

    // true si algun Pago usa este metodo (la llave foranea no deja borrarlo)
    public bool EstaEnUso(int id)
    {
        DataTable dt = Db.Consultar("SELECT COUNT(*) FROM Pago WHERE id_metodo_pago=@id", Db.P("@id", id));
        return Convert.ToInt32(dt.Rows[0][0]) > 0;
    }
}
