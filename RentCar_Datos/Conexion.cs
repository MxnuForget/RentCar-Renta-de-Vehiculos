using System.Data.SqlClient;

public class Conexion
{
    // ===== CAMBIE AQUÍ SOLO SI SU SERVIDOR ES OTRO (deje UNA línea activa) =====
    // LocalDB (viene con Visual Studio):

    private static string cadena =
        @"Server=.\SQLEXPRESS;Database=RentCar;Trusted_Connection=True;TrustServerCertificate=True;";

    // SQL Server Express:
    // private static string cadena = @"Server=.\SQLEXPRESS;Database=RentCar;Trusted_Connection=True;TrustServerCertificate=True;";
    // SQL Server normal:
    // private static string cadena = @"Server=.;Database=RentCar;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion() { return new SqlConnection(cadena); }
}
