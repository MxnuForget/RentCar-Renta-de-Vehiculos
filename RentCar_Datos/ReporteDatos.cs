using System;
using System.Data;

// Capa de datos de los REPORTES: aqui vive todo el SQL (siempre parametrizado).
// Convencion: un filtro "Todos" llega como NULL (o 0 en los ids) y la consulta lo ignora con (@x IS NULL OR ...).
public class ReporteDatos
{
    // 0 (o menos) significa "Todos" -> se envia NULL a SQL Server
    private static object IdONull(int id) { return id <= 0 ? null : (object)id; }

    // ---------- Reporte 1: alquileres por fecha y estado ----------
    public DataTable Alquileres(DateTime ini, DateTime fin, string estado)
    {
        return Db.Consultar(@"SELECT a.id_alquiler,
                                     p.nombres + ' ' + p.ap_paterno AS cliente,
                                     v.placa, v.nombre_modelo AS modelo,
                                     a.fecha_inicio, a.fecha_fin_pactada, a.fecha_devolucion_real,
                                     a.estado_alquiler AS estado
                              FROM Alquiler a
                              JOIN Persona  p ON p.id_persona  = a.id_cliente
                              JOIN Vehiculo v ON v.id_vehiculo = a.id_vehiculo
                              WHERE a.fecha_inicio BETWEEN @ini AND @fin
                                AND (@estado IS NULL OR a.estado_alquiler = @estado)
                              ORDER BY a.fecha_inicio DESC, a.id_alquiler DESC",
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@estado", estado));
    }

    // Resumen del reporte 1: cantidad de alquileres por estado
    public DataTable ResumenAlquileres(DateTime ini, DateTime fin, string estado)
    {
        return Db.Consultar(@"SELECT a.estado_alquiler AS estado, COUNT(*) AS cantidad
                              FROM Alquiler a
                              WHERE a.fecha_inicio BETWEEN @ini AND @fin
                                AND (@estado IS NULL OR a.estado_alquiler = @estado)
                              GROUP BY a.estado_alquiler
                              ORDER BY cantidad DESC",
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@estado", estado));
    }

    // ---------- Reporte 2: ingresos por pagos ----------
    public DataTable Pagos(DateTime ini, DateTime fin, int idMetodo)
    {
        return Db.Consultar(@"SELECT pg.id_alquiler, pg.num_pago, pg.fecha_pago,
                                     p.nombres + ' ' + p.ap_paterno AS cliente,
                                     m.nombre_metodo AS metodo, pg.monto
                              FROM Pago pg
                              JOIN Alquiler    a ON a.id_alquiler     = pg.id_alquiler
                              JOIN Persona     p ON p.id_persona      = a.id_cliente
                              JOIN MetodoPago  m ON m.id_metodo_pago  = pg.id_metodo_pago
                              WHERE pg.fecha_pago BETWEEN @ini AND @fin
                                AND (@met IS NULL OR pg.id_metodo_pago = @met)
                              ORDER BY pg.fecha_pago DESC, pg.id_alquiler, pg.num_pago",
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@met", IdONull(idMetodo)));
    }

    // Resumen del reporte 2: una sola fila con cantidad, suma, promedio y maximo
    public DataTable ResumenPagos(DateTime ini, DateTime fin, int idMetodo)
    {
        return Db.Consultar(@"SELECT COUNT(*) AS num_pagos,
                                     ISNULL(SUM(pg.monto), 0) AS total,
                                     ISNULL(AVG(pg.monto), 0) AS promedio,
                                     ISNULL(MAX(pg.monto), 0) AS maximo
                              FROM Pago pg
                              WHERE pg.fecha_pago BETWEEN @ini AND @fin
                                AND (@met IS NULL OR pg.id_metodo_pago = @met)",
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@met", IdONull(idMetodo)));
    }

    // ---------- Reporte 3: vehiculos mas alquilados (ranking) ----------
    // LEFT JOIN para que tambien aparezcan los vehiculos que no se alquilaron (con 0)
    public DataTable MasAlquilados(DateTime ini, DateTime fin, int idCategoria)
    {
        return Db.Consultar(@"SELECT v.placa, v.nombre_modelo AS modelo, c.nombre_categoria AS categoria,
                                     COUNT(a.id_alquiler) AS alquileres,
                                     ISNULL(SUM(DATEDIFF(DAY, a.fecha_inicio, a.fecha_fin_pactada) + 1), 0) AS dias_alquilados
                              FROM Vehiculo v
                              JOIN CategoriaVehiculo c ON c.id_categoria = v.id_categoria
                              LEFT JOIN Alquiler a ON a.id_vehiculo = v.id_vehiculo
                                   AND a.estado_alquiler <> @can
                                   AND a.fecha_inicio BETWEEN @ini AND @fin
                              WHERE (@cat IS NULL OR v.id_categoria = @cat)
                              GROUP BY v.id_vehiculo, v.placa, v.nombre_modelo, c.nombre_categoria
                              ORDER BY alquileres DESC, v.placa",
            Db.P("@ini", ini.Date), Db.P("@fin", fin.Date), Db.P("@cat", IdONull(idCategoria)),
            Db.P("@can", Estados.Cancelado));
    }

    // ---------- Reporte 4: flota por categoria (con porcentajes) ----------
    public DataTable FlotaPorCategoria(int idSucursal)
    {
        return Db.Consultar(@"SELECT c.nombre_categoria AS categoria,
                                     COUNT(*) AS vehiculos,
                                     AVG(v.tarifa_diaria) AS tarifa_promedio,
                                     CAST(100.0 * COUNT(*) / SUM(COUNT(*)) OVER () AS DECIMAL(5,1)) AS porcentaje
                              FROM Vehiculo v
                              JOIN CategoriaVehiculo c ON c.id_categoria = v.id_categoria
                              WHERE (@suc IS NULL OR v.id_sucursal = @suc)
                              GROUP BY c.nombre_categoria
                              ORDER BY vehiculos DESC, categoria",
            Db.P("@suc", IdONull(idSucursal)));
    }

    // ---------- Reporte 5: clientes frecuentes ----------
    public DataTable ClientesFrecuentes(int minimo)
    {
        return Db.Consultar(@"SELECT p.nombres + ' ' + p.ap_paterno AS cliente, p.ci,
                                     COUNT(a.id_alquiler) AS alquileres,
                                     (SELECT ISNULL(SUM(pg.monto), 0)
                                      FROM Pago pg
                                      JOIN Alquiler a2 ON a2.id_alquiler = pg.id_alquiler
                                      WHERE a2.id_cliente = c.id_persona) AS total_pagado
                              FROM Cliente c
                              JOIN Persona p ON p.id_persona = c.id_persona
                              LEFT JOIN Alquiler a ON a.id_cliente = c.id_persona AND a.estado_alquiler <> @can
                              GROUP BY c.id_persona, p.nombres, p.ap_paterno, p.ci
                              HAVING COUNT(a.id_alquiler) >= @min
                              ORDER BY alquileres DESC, total_pagado DESC",
            Db.P("@min", minimo), Db.P("@can", Estados.Cancelado));
    }

    // Lista de categorias para el filtro del reporte 3 (columnas Id y Nombre, como CatalogoDatos)
    public DataTable Categorias()
    {
        return Db.Consultar("SELECT id_categoria AS Id, nombre_categoria AS Nombre FROM CategoriaVehiculo ORDER BY nombre_categoria");
    }
}
