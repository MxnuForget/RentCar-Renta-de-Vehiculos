using System.Data;
public class AlquilerDatos
{
    public void Insertar(Alquiler a)
    {
        string cols = "fecha_inicio, fecha_fin_pactada, fecha_devolucion_real, estado_alquiler, id_cliente, id_empleado, id_vehiculo";
        string vals = "@ini, @fin, @dev, @est, @cli, @emp, @veh";
        string sql = Db.EsIdentity("Alquiler", "id_alquiler")
            ? "INSERT INTO Alquiler (" + cols + ") VALUES (" + vals + ")"
            : "INSERT INTO Alquiler (id_alquiler, " + cols + ") SELECT ISNULL(MAX(id_alquiler), 0) + 1, " + vals + " FROM Alquiler";
        Db.Ejecutar(sql,
            Db.P("@ini", a.FechaInicio), Db.P("@fin", a.FechaFinPactada), Db.P("@dev", a.FechaDevolucionReal),
            Db.P("@est", a.EstadoAlquiler), Db.P("@cli", a.IdCliente), Db.P("@emp", a.IdEmpleado), Db.P("@veh", a.IdVehiculo));
    }
    public void Modificar(Alquiler a)
    {
        Db.Ejecutar(@"UPDATE Alquiler SET fecha_inicio=@ini, fecha_fin_pactada=@fin, fecha_devolucion_real=@dev,
                      estado_alquiler=@est, id_cliente=@cli, id_empleado=@emp, id_vehiculo=@veh
                      WHERE id_alquiler=@id",
            Db.P("@ini", a.FechaInicio), Db.P("@fin", a.FechaFinPactada), Db.P("@dev", a.FechaDevolucionReal),
            Db.P("@est", a.EstadoAlquiler), Db.P("@cli", a.IdCliente), Db.P("@emp", a.IdEmpleado),
            Db.P("@veh", a.IdVehiculo), Db.P("@id", a.IdAlquiler));
    }
    public void Eliminar(int id)
    {
        Db.Ejecutar("DELETE FROM Alquiler WHERE id_alquiler=@id", Db.P("@id", id));
    }
    public DataTable Listar()
    {
        return Db.Consultar(@"SELECT a.id_alquiler, a.id_cliente, a.id_empleado, a.id_vehiculo,
                                     pc.nombres + ' ' + pc.ap_paterno AS cliente,
                                     pe.nombres + ' ' + pe.ap_paterno AS empleado,
                                     v.placa AS vehiculo,
                                     a.fecha_inicio, a.fecha_fin_pactada, a.fecha_devolucion_real, a.estado_alquiler
                              FROM Alquiler a
                              JOIN Persona pc ON pc.id_persona = a.id_cliente
                              JOIN Persona pe ON pe.id_persona = a.id_empleado
                              JOIN Vehiculo v ON v.id_vehiculo = a.id_vehiculo
                              ORDER BY a.id_alquiler DESC");
    }
}
