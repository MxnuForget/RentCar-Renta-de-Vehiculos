using System;
public class EmpleadoRegistro
{
    public string Nombres { get; set; }
    public string ApPaterno { get; set; }
    public string ApMaterno { get; set; }
    public string Ci { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Email { get; set; }
    public decimal Salario { get; set; }
    public int IdCargo { get; set; }
    public int IdSucursal { get; set; }
    public string Rol { get; set; }
    public string Usuario { get; set; }
    public string Clave { get; set; }          // solo viaja hasta Negocio, donde se convierte en hash
}
