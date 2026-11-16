using System;

public class ClienteNegocio
{
    private ClienteAccesoDatos datos = new ClienteAccesoDatos();
    private ClienteDatos crud = new ClienteDatos();

    public void Registrar(ClienteRegistro r, string repetirClave)
    {
        r.Nombres = Validaciones.Capitalizar(r.Nombres);
        r.ApPaterno = Validaciones.Capitalizar(r.ApPaterno);
        r.ApMaterno = Validaciones.Capitalizar(r.ApMaterno);
        r.Ci = Validaciones.NormalizarCi(r.Ci);
        if (string.IsNullOrWhiteSpace(r.Nombres)) throw new Exception("Los nombres son obligatorios.");
        if (string.IsNullOrWhiteSpace(r.ApPaterno)) throw new Exception("El apellido paterno es obligatorio.");
        Validaciones.ValidarCi(r.Ci);
        if (r.FechaNacimiento.Date > DateTime.Today.AddYears(-18))
            throw new Exception("Debe ser mayor de 18 años para alquilar un auto.");
        if (!string.IsNullOrWhiteSpace(r.Email) && (!r.Email.Contains("@") || !r.Email.Contains(".")))
            throw new Exception("El correo no es válido.");
        if (string.IsNullOrWhiteSpace(r.Licencia)) throw new Exception("El número de licencia es obligatorio.");
        if (r.FechaVencLicencia.Date <= DateTime.Today) throw new Exception("La licencia está vencida.");
        Validaciones.ValidarPersonaUnica(r.Ci, r.Email, 0);
        Validaciones.ValidarUsuarioLibre(r.Usuario);      // un usuario no se puede repetir en todo el sistema
        Seguridad.ValidarClave(r.Clave);
        if (r.Clave != repetirClave) throw new Exception("Las contraseñas no coinciden.");

        r.Usuario = r.Usuario.Trim();
        datos.Registrar(r, Seguridad.Hash(r.Clave));   // se guarda el hash, nunca la clave
    }

    // ---------- Gestión de clientes (CRUD, personal de la empresa) ----------
    private static void Validar(Cliente c)
    {
        if (c.IdPersona <= 0) throw new Exception("Seleccione una persona.");
        if (string.IsNullOrWhiteSpace(c.LicenciaConducir)) throw new Exception("El número de licencia es obligatorio.");
        if (c.FechaVencLicencia.Date <= DateTime.Today) throw new Exception("La licencia está vencida.");
    }
    public void Registrar(Cliente c) { Permisos.RequerirEmpleado(); Validar(c); crud.Insertar(c); }
    public void Modificar(Cliente c)
    {
        Permisos.RequerirEmpleado();
        if (c.IdPersona <= 0) throw new Exception("Seleccione un cliente de la tabla.");
        Validar(c); crud.Modificar(c);
    }
    public void Eliminar(int id)
    {
        Permisos.RequerirEmpleado();
        if (id <= 0) throw new Exception("Seleccione un cliente de la tabla.");
        crud.Eliminar(id);
    }
    public System.Data.DataTable Listar() { Permisos.RequerirEmpleado(); return crud.Listar(); }
}
