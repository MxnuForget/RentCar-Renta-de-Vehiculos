public class UsuarioLogin
{
    public int IdEmpleado { get; set; }
    public string Usuario { get; set; }
    public string ContrasenaHash { get; set; }   // nunca la contraseña en texto plano
    public bool Activo { get; set; }
    public string Rol { get; set; }              // Administrador u Operador
}
