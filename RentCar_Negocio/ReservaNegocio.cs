using System;
using System.Data;

public class ReservaNegocio
{
    private ReservaDatos datos = new ReservaDatos();

    private static void ValidarFechas(DateTime desde, DateTime hasta)
    {
        if (desde.Date < DateTime.Today) throw new Exception("La fecha de inicio no puede ser anterior a hoy.");
        if (hasta.Date < desde.Date) throw new Exception("La fecha final no puede ser anterior a la de inicio.");
    }

    public DataTable VehiculosDisponibles(DateTime desde, DateTime hasta)
    {
        ValidarFechas(desde, hasta);
        return datos.Disponibles(desde, hasta);
    }

    public void Reservar(int idCliente, int idVehiculo, DateTime desde, DateTime hasta)
    {
        if (idCliente <= 0) throw new Exception("Inicie sesión como cliente.");
        if (idVehiculo <= 0) throw new Exception("Seleccione un auto de la lista.");
        ValidarFechas(desde, hasta);
        datos.Reservar(idCliente, idVehiculo, desde, hasta);
    }

    public DataTable MisReservas(int idCliente) { return datos.MisReservas(idCliente); }

    public void Cancelar(int idAlquiler, int idCliente)
    {
        if (idAlquiler <= 0) throw new Exception("Seleccione una reserva.");
        if (datos.Cancelar(idAlquiler, idCliente) == 0)
            throw new Exception("Solo se pueden cancelar reservas Activas que todavía no empiezan.");
    }
}
