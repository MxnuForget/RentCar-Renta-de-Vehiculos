using System.Data;

// Consultas de apoyo para validar antes de guardar (usuario, CI y correo repetidos)
public class ValidacionDatos
{
    // ¿Ya existe ese nombre de usuario en CUALQUIER cuenta (empleado o cliente)?
    public bool UsuarioExiste(string usuario)
    {
        DataTable dt = Db.Consultar(@"SELECT COUNT(*) FROM (
                                          SELECT usuario FROM Empleado WHERE usuario = @u
                                          UNION ALL
                                          SELECT usuario FROM CuentaCliente WHERE usuario = @u) x",
            Db.P("@u", usuario));
        return System.Convert.ToInt32(dt.Rows[0][0]) > 0;
    }

    // ¿Ya existe otra persona con ese CI? (idExcluida = la persona que se está modificando, 0 si es nueva)
    public bool CiExiste(string ci, int idExcluida)
    {
        DataTable dt = Db.Consultar("SELECT COUNT(*) FROM Persona WHERE ci = @ci AND id_persona <> @id",
            Db.P("@ci", ci), Db.P("@id", idExcluida));
        return System.Convert.ToInt32(dt.Rows[0][0]) > 0;
    }

    // ¿Ya existe otra persona con ese correo?
    public bool EmailExiste(string email, int idExcluida)
    {
        DataTable dt = Db.Consultar("SELECT COUNT(*) FROM Persona WHERE email = @m AND id_persona <> @id",
            Db.P("@m", email), Db.P("@id", idExcluida));
        return System.Convert.ToInt32(dt.Rows[0][0]) > 0;
    }
}
