using System.Data;
public class PagoDatos
{
    public void Insertar(Pago p)
    {
        // num_pago = siguiente número dentro del alquiler (PK compuesta)
        Db.Ejecutar(@"INSERT INTO Pago (id_alquiler, num_pago, fecha_pago, monto, id_metodo_pago)
                      SELECT @alq, ISNULL(MAX(num_pago), 0) + 1, @fecha, @monto, @met
                      FROM Pago WHERE id_alquiler = @alq",
            Db.P("@alq", p.IdAlquiler), Db.P("@fecha", p.FechaPago), Db.P("@monto", p.Monto), Db.P("@met", p.IdMetodoPago));
    }
    public void Modificar(Pago p)
    {
        Db.Ejecutar(@"UPDATE Pago SET fecha_pago=@fecha, monto=@monto, id_metodo_pago=@met
                      WHERE id_alquiler=@alq AND num_pago=@num",
            Db.P("@fecha", p.FechaPago), Db.P("@monto", p.Monto), Db.P("@met", p.IdMetodoPago),
            Db.P("@alq", p.IdAlquiler), Db.P("@num", p.NumPago));
    }
    public void Eliminar(int idAlquiler, int numPago)
    {
        Db.Ejecutar("DELETE FROM Pago WHERE id_alquiler=@alq AND num_pago=@num",
            Db.P("@alq", idAlquiler), Db.P("@num", numPago));
    }
    public DataTable Listar()
    {
        return Db.Consultar(@"SELECT p.id_alquiler, p.num_pago, p.fecha_pago, p.monto, p.id_metodo_pago, m.nombre_metodo AS metodo_pago
                              FROM Pago p JOIN MetodoPago m ON m.id_metodo_pago = p.id_metodo_pago
                              ORDER BY p.id_alquiler DESC, p.num_pago");
    }
}
