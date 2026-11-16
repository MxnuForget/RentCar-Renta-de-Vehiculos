using System;
using System.Globalization;
using System.Text.RegularExpressions;

// Reglas de validación compartidas por todos los módulos
public static class Validaciones
{
    // "juan carlos  PEREZ" -> "Juan Carlos Perez"  (solo nombres y apellidos)
    public static string Capitalizar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return "";
        string limpio = Regex.Replace(texto.Trim(), @"\s+", " ");
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(limpio.ToLower());
    }

    public static string NormalizarCi(string ci)
    {
        return string.IsNullOrWhiteSpace(ci) ? "" : ci.Trim().ToUpperInvariant();
    }

    // CI boliviano: de 5 a 10 dígitos y, opcionalmente, complemento con guion (ej.: 1234567-1A)
    public static void ValidarCi(string ci)
    {
        if (string.IsNullOrWhiteSpace(ci)) throw new Exception("El número de carnet (CI) es obligatorio.");
        if (!Regex.IsMatch(ci.Trim(), @"^[0-9]{5,10}(-[0-9A-Za-z]{1,2})?$"))
            throw new Exception("El CI no es válido. Use solo números (de 5 a 10 dígitos); si tiene complemento, escríbalo con guion (ej.: 1234567-1A).");
    }

    // Un nombre de usuario no puede repetirse en todo el sistema (empleados y clientes)
    public static void ValidarUsuarioLibre(string usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario) || usuario.Trim().Length < 4)
            throw new Exception("El usuario debe tener al menos 4 caracteres.");
        if (usuario.Trim().Contains(" "))
            throw new Exception("El usuario no puede contener espacios.");
        if (new ValidacionDatos().UsuarioExiste(usuario.Trim()))
            throw new Exception("El nombre de usuario \"" + usuario.Trim() + "\" no está disponible. Elija otro.");
    }

    // CI y correo no repetidos (idExcluida = persona que se modifica; 0 si es nueva)
    public static void ValidarPersonaUnica(string ci, string email, int idExcluida)
    {
        ValidacionDatos v = new ValidacionDatos();
        if (v.CiExiste(ci, idExcluida))
            throw new Exception("Ya existe una persona registrada con el CI " + ci + ".");
        if (!string.IsNullOrWhiteSpace(email) && v.EmailExiste(email.Trim(), idExcluida))
            throw new Exception("Ya existe una persona registrada con ese correo electrónico.");
    }
}
