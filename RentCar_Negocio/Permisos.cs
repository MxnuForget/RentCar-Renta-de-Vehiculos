using System;

// Control de acceso en la capa de negocio (no depende de que la pantalla oculte botones)
public static class Permisos
{
    public static void RequerirEmpleado()
    {
        if (Sesion.Rol != Roles.Administrador && Sesion.Rol != Roles.Operador)
            throw new Exception("No tiene permisos para esta operación (solo personal de la empresa).");
    }

    public static void RequerirAdministrador()
    {
        if (!Sesion.EsAdministrador)
            throw new Exception("No tiene permisos para esta operación (solo el Administrador).");
    }
}
