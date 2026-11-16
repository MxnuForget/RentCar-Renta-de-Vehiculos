using System;
using System.Data;

// Cuentas de clientes: inicio de sesión y registro (tabla CuentaCliente)
public class ClienteAccesoDatos
{
    public ClienteLogin ObtenerPorUsuario(string usuario)
    {
        DataTable dt = Db.Consultar(@"SELECT c.id_persona, c.usuario, c.contrasena, c.activo,
                                             p.nombres + ' ' + p.ap_paterno AS nombre
                                      FROM CuentaCliente c JOIN Persona p ON p.id_persona = c.id_persona
                                      WHERE c.usuario = @u", Db.P("@u", usuario));
        if (dt.Rows.Count == 0) return null;
        DataRow r = dt.Rows[0];
        return new ClienteLogin
        {
            IdCliente = Convert.ToInt32(r["id_persona"]),
            Usuario = Convert.ToString(r["usuario"]),
            ContrasenaHash = Convert.ToString(r["contrasena"]),
            NombreCompleto = Convert.ToString(r["nombre"]),
            Activo = Convert.ToBoolean(r["activo"])
        };
    }

    // Crea Persona + Cliente + CuentaCliente en una sola transacción
    public void Registrar(ClienteRegistro r, string hash)
    {
        Db.Ejecutar(@"BEGIN TRY
                          BEGIN TRANSACTION;
                          INSERT INTO Persona (nombres, ap_paterno, ap_materno, ci, fecha_nacimiento, email)
                          VALUES (@nom, @pat, @mat, @ci, @nac, @mail);
                          DECLARE @id INT = SCOPE_IDENTITY();
                          INSERT INTO Cliente (id_persona, licencia_conducir, fecha_venc_licencia)
                          VALUES (@id, @lic, @venc);
                          INSERT INTO CuentaCliente (id_persona, usuario, contrasena)
                          VALUES (@id, @usr, @hash);
                          COMMIT TRANSACTION;
                      END TRY
                      BEGIN CATCH
                          IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
                          THROW;
                      END CATCH",
            Db.P("@nom", r.Nombres), Db.P("@pat", r.ApPaterno),
            Db.P("@mat", string.IsNullOrWhiteSpace(r.ApMaterno) ? null : r.ApMaterno),
            Db.P("@ci", r.Ci), Db.P("@nac", r.FechaNacimiento),
            Db.P("@mail", string.IsNullOrWhiteSpace(r.Email) ? null : r.Email),
            Db.P("@lic", r.Licencia), Db.P("@venc", r.FechaVencLicencia),
            Db.P("@usr", r.Usuario), Db.P("@hash", hash));
    }
}
