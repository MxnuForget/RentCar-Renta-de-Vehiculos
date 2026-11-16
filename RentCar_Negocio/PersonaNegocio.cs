using System;
using System.Data;
public class PersonaNegocio
{
    private PersonaDatos datos = new PersonaDatos();
    private void Validar(Persona p)
    {
        p.Nombres = Validaciones.Capitalizar(p.Nombres);
        p.ApPaterno = Validaciones.Capitalizar(p.ApPaterno);
        p.ApMaterno = Validaciones.Capitalizar(p.ApMaterno);
        p.Ci = Validaciones.NormalizarCi(p.Ci);
        if (string.IsNullOrWhiteSpace(p.Nombres)) throw new Exception("Los nombres son obligatorios.");
        if (string.IsNullOrWhiteSpace(p.ApPaterno)) throw new Exception("El apellido paterno es obligatorio.");
        Validaciones.ValidarCi(p.Ci);
        if (p.FechaNacimiento.Date >= DateTime.Today) throw new Exception("La fecha de nacimiento no es válida.");
        if (!string.IsNullOrWhiteSpace(p.Email) && (!p.Email.Contains("@") || !p.Email.Contains(".")))
            throw new Exception("El correo no es válido.");
        Validaciones.ValidarPersonaUnica(p.Ci, p.Email, p.IdPersona);
    }
    public void Registrar(Persona p) { Permisos.RequerirAdministrador(); Validar(p); datos.Insertar(p); }
    public void Modificar(Persona p)
    {
        Permisos.RequerirAdministrador();
        if (p.IdPersona <= 0) throw new Exception("Seleccione una persona de la tabla.");
        Validar(p); datos.Modificar(p);
    }
    public void Eliminar(int id)
    {
        Permisos.RequerirAdministrador();
        if (id <= 0) throw new Exception("Seleccione una persona de la tabla.");
        datos.Eliminar(id);
    }
    public DataTable Listar() { Permisos.RequerirAdministrador(); return datos.Listar(); }
}
