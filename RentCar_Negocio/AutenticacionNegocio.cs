using System;

public static class Sesion
{
    public static string Rol { get; set; }              // Administrador, Operador o Cliente
    public static int IdEmpleado { get; set; }
    public static int IdCliente { get; set; }
    public static string Usuario { get; set; }
    public static string NombreCompleto { get; set; }
    public static bool CerroSesion { get; set; }        // true si el usuario pulsó "Cerrar sesión"

    public static bool EsAdministrador { get { return Rol == Roles.Administrador; } }

    public static void Cerrar()
    {
        Rol = null; IdEmpleado = 0; IdCliente = 0; Usuario = null; NombreCompleto = null; CerroSesion = false;
    }
}

public class AutenticacionNegocio
{
    private UsuarioDatos datos = new UsuarioDatos();
    private ClienteAccesoDatos clientes = new ClienteAccesoDatos();

    private static void Pedir(string usuario, string clave)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(clave))
            throw new Exception("Ingrese usuario y contraseña.");
    }

    // Inicio de sesión ÚNICO: el sistema detecta solo si la cuenta es de empleado o de cliente
    public bool IniciarSesion(string usuario, string clave)
    {
        Pedir(usuario, clave);
        if (Login(usuario, clave)) return true;       // primero busca entre los empleados
        return LoginCliente(usuario, clave);          // si no es empleado, busca entre los clientes
    }

    // Empleados (Administrador u Operador)
    public bool Login(string usuario, string clave)
    {
        Pedir(usuario, clave);
        UsuarioLogin u = datos.ObtenerPorUsuario(usuario);
        if (u == null || !Seguridad.Verificar(clave, u.ContrasenaHash))
            return false;                      // mismo resultado si falla el usuario o la clave
        if (!u.Activo)                         // solo se revela después de acertar la clave
            throw new Exception("La cuenta está desactivada. Contacte al administrador.");

        Sesion.Rol = u.Rol == Roles.Administrador ? Roles.Administrador : Roles.Operador;
        Sesion.IdEmpleado = u.IdEmpleado;
        Sesion.Usuario = u.Usuario;
        Sesion.NombreCompleto = u.Usuario;
        return true;
    }

    // Clientes
    public bool LoginCliente(string usuario, string clave)
    {
        Pedir(usuario, clave);
        ClienteLogin c = clientes.ObtenerPorUsuario(usuario);
        if (c == null || !Seguridad.Verificar(clave, c.ContrasenaHash))
            return false;
        if (!c.Activo)
            throw new Exception("La cuenta está desactivada. Contacte a la empresa.");

        Sesion.Rol = Roles.Cliente;
        Sesion.IdCliente = c.IdCliente;
        Sesion.Usuario = c.Usuario;
        Sesion.NombreCompleto = c.NombreCompleto;
        return true;
    }

    // Crea o migra la contraseña (hash) de un empleado existente
    public void EstablecerClave(string usuario, string clave)
    {
        if (string.IsNullOrWhiteSpace(usuario)) throw new Exception("Usuario obligatorio.");
        Seguridad.ValidarClave(clave);
        if (datos.ActualizarClave(usuario, Seguridad.Hash(clave)) == 0)
            throw new Exception("No existe ese usuario.");
    }
}
