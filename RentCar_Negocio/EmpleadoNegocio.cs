using System;
using System.Data;

// Cuentas de empleados: solo el Administrador puede crearlas o activarlas/desactivarlas
public class EmpleadoNegocio
{
    private EmpleadoDatos datos = new EmpleadoDatos();

    public void Crear(EmpleadoRegistro r, string repetirClave)
    {
        Permisos.RequerirAdministrador();
        r.Nombres = Validaciones.Capitalizar(r.Nombres);
        r.ApPaterno = Validaciones.Capitalizar(r.ApPaterno);
        r.ApMaterno = Validaciones.Capitalizar(r.ApMaterno);
        r.Ci = Validaciones.NormalizarCi(r.Ci);
        if (string.IsNullOrWhiteSpace(r.Nombres)) throw new Exception("Los nombres son obligatorios.");
        if (string.IsNullOrWhiteSpace(r.ApPaterno)) throw new Exception("El apellido paterno es obligatorio.");
        Validaciones.ValidarCi(r.Ci);
        if (r.FechaNacimiento.Date > DateTime.Today.AddYears(-18)) throw new Exception("El empleado debe ser mayor de 18 años.");
        if (!string.IsNullOrWhiteSpace(r.Email) && (!r.Email.Contains("@") || !r.Email.Contains(".")))
            throw new Exception("El correo no es válido.");
        if (r.Salario < 0) throw new Exception("El salario no puede ser negativo.");
        if (r.IdCargo <= 0) throw new Exception("Seleccione un cargo.");
        if (r.IdSucursal <= 0) throw new Exception("Seleccione una sucursal.");
        if (r.Rol != Roles.Administrador && r.Rol != Roles.Operador) throw new Exception("Seleccione el rol del empleado.");
        Validaciones.ValidarPersonaUnica(r.Ci, r.Email, 0);
        Validaciones.ValidarUsuarioLibre(r.Usuario);      // un usuario no se puede repetir en todo el sistema
        Seguridad.ValidarClave(r.Clave);
        if (r.Clave != repetirClave) throw new Exception("Las contraseñas no coinciden.");

        r.Usuario = r.Usuario.Trim();
        datos.Crear(r, Seguridad.Hash(r.Clave));
    }

    public DataTable Listar()
    {
        Permisos.RequerirAdministrador();
        return datos.Listar();
    }

    public void CambiarActivo(int id, bool activo)
    {
        Permisos.RequerirAdministrador();
        if (id <= 0) throw new Exception("Seleccione un empleado de la tabla.");
        if (!activo)
        {
            if (id == Sesion.IdEmpleado) throw new Exception("No puede desactivar su propia cuenta.");
            if (datos.RolDe(id) == Roles.Administrador && datos.ContarAdministradoresActivos() <= 1)
                throw new Exception("Debe quedar al menos un Administrador activo.");
        }
        if (datos.CambiarActivo(id, activo) == 0) throw new Exception("No se pudo cambiar el estado de la cuenta.");
    }
}
