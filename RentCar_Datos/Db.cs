using System;
using System.Data;
using System.Data.SqlClient;

// Ayudante: ejecuta SQL parametrizado. Todo el SQL del sistema vive en la capa Datos.
public static class Db
{
    public static int Ejecutar(string sql, params SqlParameter[] ps)
    {
        try
        {
            using (SqlConnection cn = Conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(ps);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public static DataTable Consultar(string sql, params SqlParameter[] ps)
    {
        try
        {
            using (SqlConnection cn = Conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(ps);
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public static SqlParameter P(string nombre, object valor)
    {
        return new SqlParameter(nombre, valor ?? DBNull.Value);
    }

    // ¿La columna es IDENTITY? (así funciona aunque su script la haya creado con o sin IDENTITY)
    public static bool EsIdentity(string tabla, string columna)
    {
        DataTable dt = Consultar("SELECT COLUMNPROPERTY(OBJECT_ID(@t), @c, 'IsIdentity') AS v",
            P("@t", "dbo." + tabla), P("@c", columna));
        return dt.Rows.Count > 0 && dt.Rows[0]["v"] != DBNull.Value && Convert.ToInt32(dt.Rows[0]["v"]) == 1;
    }

    // Convierte errores técnicos de SQL Server en mensajes entendibles
    private static Exception Traducir(SqlException ex)
    {
        if (ex.Number == 547)
        {
            if (ex.Message.IndexOf("CHECK", StringComparison.OrdinalIgnoreCase) >= 0)
                return new Exception("Un valor no cumple las reglas de la tabla. Detalle: " + ex.Message);
            return new Exception("La operación no se puede realizar porque el registro está relacionado con otros datos.");
        }
        if (ex.Number == 2627 || ex.Number == 2601)
            return new Exception("Ya existe un registro con ese dato (CI, correo, licencia o usuario repetido).");
        if (ex.Number == 207)
            return new Exception("Falta una columna en la base de datos (" + ex.Message + "). Ejecute BaseDeDatos\\MejorasAutenticacion.sql en SSMS.");
        if (ex.Number == 208)
            return new Exception("Falta crear algo en la base de datos (" + ex.Message + "). Ejecute BaseDeDatos\\RolesCliente.sql en SSMS.");
        return ex;
    }
}
