using System;
using System.Data;

// Gestión de cuentas de empleados (solo la usa el Administrador)
public class EmpleadoDatos
{
    // Crea Persona + Empleado en una sola transacción
    public void Crear(EmpleadoRegistro r, string hash)
    {
        Db.Ejecutar(@"BEGIN TRY
                          BEGIN TRANSACTION;
                          INSERT INTO Persona (nombres, ap_paterno, ap_materno, ci, fecha_nacimiento, email)
                          VALUES (@nom, @pat, @mat, @ci, @nac, @mail);
                          DECLARE @id INT = SCOPE_IDENTITY();
                          INSERT INTO Empleado (id_persona, fecha_contratacion, salario, usuario, contrasena, id_cargo, id_sucursal, rol)
                          VALUES (@id, CAST(GETDATE() AS DATE), @sal, @usr, @hash, @cargo, @suc, @rol);
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
            Db.P("@sal", r.Salario), Db.P("@usr", r.Usuario), Db.P("@hash", hash),
            Db.P("@cargo", r.IdCargo), Db.P("@suc", r.IdSucursal), Db.P("@rol", r.Rol));
    }

    public DataTable Listar()
    {
        return Db.Consultar(@"SELECT e.id_persona, p.nombres + ' ' + p.ap_paterno AS nombre, e.usuario, e.rol,
                                     c.nombre_cargo AS cargo, s.nombre_sucursal AS sucursal,
                                     e.activo, e.fecha_creacion
                              FROM Empleado e
                              JOIN Persona p ON p.id_persona = e.id_persona
                              JOIN Cargo c ON c.id_cargo = e.id_cargo
                              JOIN Sucursal s ON s.id_sucursal = e.id_sucursal
                              WHERE e.usuario <> 'reservas_web'
                              ORDER BY e.activo DESC, e.usuario");
    }

    public int CambiarActivo(int id, bool activo)
    {
        return Db.Ejecutar("UPDATE Empleado SET activo = @a WHERE id_persona = @id AND usuario <> 'reservas_web'",
            Db.P("@a", activo), Db.P("@id", id));
    }

    public string RolDe(int id)
    {
        DataTable dt = Db.Consultar("SELECT rol FROM Empleado WHERE id_persona = @id", Db.P("@id", id));
        return dt.Rows.Count == 0 ? null : Convert.ToString(dt.Rows[0][0]);
    }

    public int ContarAdministradoresActivos()
    {
        DataTable dt = Db.Consultar("SELECT COUNT(*) FROM Empleado WHERE rol = 'Administrador' AND activo = 1");
        return Convert.ToInt32(dt.Rows[0][0]);
    }
}
