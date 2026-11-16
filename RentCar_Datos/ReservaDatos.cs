using System;
using System.Data;

// Lo que hace el cliente: buscar autos libres, reservar, ver y cancelar sus reservas
public class ReservaDatos
{
    // Hay choque si el auto tiene un alquiler Activo que se cruza con las fechas pedidas
    private static string Choque(string vehiculo)
    {
        return @"EXISTS (SELECT 1 FROM Alquiler a
                         WHERE a.id_vehiculo = " + vehiculo + @"
                           AND a.estado_alquiler = @activo
                           AND a.fecha_inicio <= @fin
                           AND ISNULL(a.fecha_devolucion_real, a.fecha_fin_pactada) >= @ini)";
    }

    public DataTable Disponibles(DateTime ini, DateTime fin)
    {
        return Db.Consultar(@"SELECT v.id_vehiculo, v.placa, v.nombre_modelo AS modelo, v.color,
                                     v.anio_fabricacion AS anio, v.tarifa_diaria AS tarifa_dia,
                                     v.tarifa_diaria * (DATEDIFF(DAY, @ini, @fin) + 1) AS total_estimado
                              FROM Vehiculo v
                              WHERE v.estado <> 'Mantenimiento' AND NOT " + Choque("v.id_vehiculo") + @"
                              ORDER BY v.tarifa_diaria, v.placa",
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@activo", Estados.Activo));
    }

    // Las reservas online se registran a nombre de un empleado interno (usuario 'reservas_web')
    private int IdEmpleadoWeb()
    {
        DataTable dt = Db.Consultar("SELECT id_persona FROM Empleado WHERE usuario = 'reservas_web'");
        if (dt.Rows.Count == 0)
            throw new Exception("Falta el usuario interno de reservas. Ejecute BaseDeDatos\\RolesCliente.sql en SSMS.");
        return Convert.ToInt32(dt.Rows[0][0]);
    }

    public void Reservar(int idCliente, int idVehiculo, DateTime ini, DateTime fin)
    {
        int idEmp = IdEmpleadoWeb();
        string cols = "fecha_inicio, fecha_fin_pactada, fecha_devolucion_real, estado_alquiler, id_cliente, id_empleado, id_vehiculo";
        string vals = "@ini, @fin, NULL, @activo, @cli, @emp, @veh";
        string sql = Db.EsIdentity("Alquiler", "id_alquiler")
            ? "INSERT INTO Alquiler (" + cols + ") SELECT " + vals + " WHERE NOT " + Choque("@veh")
            : "INSERT INTO Alquiler (id_alquiler, " + cols + ") SELECT ISNULL((SELECT MAX(id_alquiler) FROM Alquiler), 0) + 1, " + vals + " WHERE NOT " + Choque("@veh");

        int filas = Db.Ejecutar(sql,
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@activo", Estados.Activo),
            Db.P("@cli", idCliente), Db.P("@emp", idEmp), Db.P("@veh", idVehiculo));
        if (filas == 0) throw new Exception("El vehículo ya no está disponible en esas fechas. Busque de nuevo.");
    }

    public DataTable MisReservas(int idCliente)
    {
        return Db.Consultar(@"SELECT a.id_alquiler, v.placa, v.nombre_modelo AS modelo,
                                     a.fecha_inicio, a.fecha_fin_pactada, a.estado_alquiler,
                                     v.tarifa_diaria * (DATEDIFF(DAY, a.fecha_inicio, a.fecha_fin_pactada) + 1) AS total_estimado
                              FROM Alquiler a JOIN Vehiculo v ON v.id_vehiculo = a.id_vehiculo
                              WHERE a.id_cliente = @cli
                              ORDER BY a.fecha_inicio DESC, a.id_alquiler DESC",
            Db.P("@cli", idCliente));
    }

    // Solo reservas propias, en estado Activo y que aún no empiezan
    public int Cancelar(int idAlquiler, int idCliente)
    {
        return Db.Ejecutar(@"UPDATE Alquiler SET estado_alquiler = @can
                             WHERE id_alquiler = @id AND id_cliente = @cli
                               AND estado_alquiler = @activo
                               AND fecha_inicio >= CAST(GETDATE() AS DATE)",
            Db.P("@can", Estados.Cancelado), Db.P("@activo", Estados.Activo),
            Db.P("@id", idAlquiler), Db.P("@cli", idCliente));
    }
}
