using System;
using System.Data;

// Panel del administrador: ver alquileres con filtros y cambiar su estado
public class AlquilerAdminDatos
{
    public DataTable Filtrar(string estado, DateTime? desde, DateTime? hasta, string texto)
    {
        return Db.Consultar(@"SELECT t.*, t.total_estimado - t.pagado AS saldo
                              FROM (
                                  SELECT a.id_alquiler,
                                         pc.nombres + ' ' + pc.ap_paterno AS cliente,
                                         pe.nombres + ' ' + pe.ap_paterno AS empleado,
                                         v.placa, v.nombre_modelo AS modelo,
                                         a.fecha_inicio, a.fecha_fin_pactada, a.fecha_devolucion_real, a.estado_alquiler,
                                         v.tarifa_diaria * (DATEDIFF(DAY, a.fecha_inicio, a.fecha_fin_pactada) + 1) AS total_estimado,
                                         ISNULL(pg.pagado, 0) AS pagado
                                  FROM Alquiler a
                                  JOIN Persona pc ON pc.id_persona = a.id_cliente
                                  JOIN Persona pe ON pe.id_persona = a.id_empleado
                                  JOIN Vehiculo v ON v.id_vehiculo = a.id_vehiculo
                                  LEFT JOIN (SELECT id_alquiler, SUM(monto) AS pagado FROM Pago GROUP BY id_alquiler) pg
                                         ON pg.id_alquiler = a.id_alquiler
                                  WHERE (@estado IS NULL OR a.estado_alquiler = @estado)
                                    AND (@desde  IS NULL OR a.fecha_inicio >= @desde)
                                    AND (@hasta  IS NULL OR a.fecha_inicio <= @hasta)
                                    AND (@texto  IS NULL OR (pc.nombres + ' ' + pc.ap_paterno) LIKE '%' + @texto + '%'
                                                         OR v.placa LIKE '%' + @texto + '%')
                              ) t
                              ORDER BY t.fecha_inicio DESC, t.id_alquiler DESC",
            Db.P("@estado", estado), Db.P("@desde", desde), Db.P("@hasta", hasta), Db.P("@texto", texto));
    }

    public int Finalizar(int id)
    {
        return Db.Ejecutar(@"UPDATE Alquiler
                             SET estado_alquiler = @fin,
                                 fecha_devolucion_real = ISNULL(fecha_devolucion_real, CAST(GETDATE() AS DATE))
                             WHERE id_alquiler = @id AND estado_alquiler = @activo",
            Db.P("@fin", Estados.Finalizado), Db.P("@activo", Estados.Activo), Db.P("@id", id));
    }

    public int Cancelar(int id)
    {
        return Db.Ejecutar("UPDATE Alquiler SET estado_alquiler = @can WHERE id_alquiler = @id AND estado_alquiler = @activo",
            Db.P("@can", Estados.Cancelado), Db.P("@activo", Estados.Activo), Db.P("@id", id));
    }
}
