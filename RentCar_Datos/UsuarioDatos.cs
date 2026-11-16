using System.Data;
// Autenticación de empleados: lee/actualiza usuario, contraseña (hash), rol y estado de la tabla Empleado.
public class UsuarioDatos
{
    public UsuarioLogin ObtenerPorUsuario(string usuario)
    {
        DataTable dt = Db.Consultar("SELECT id_persona, usuario, contrasena, activo, rol FROM Empleado WHERE usuario = @u",
            Db.P("@u", usuario));
        if (dt.Rows.Count == 0) return null;
        DataRow r = dt.Rows[0];
        return new UsuarioLogin
        {
            IdEmpleado = System.Convert.ToInt32(r["id_persona"]),
            Usuario = System.Convert.ToString(r["usuario"]),
            ContrasenaHash = System.Convert.ToString(r["contrasena"]),
            Activo = System.Convert.ToBoolean(r["activo"]),
            Rol = System.Convert.ToString(r["rol"])
        };
    }
    public int ActualizarClave(string usuario, string hash)
    {
        return Db.Ejecutar("UPDATE Empleado SET contrasena = @h WHERE usuario = @u", Db.P("@h", hash), Db.P("@u", usuario));
    }
}
