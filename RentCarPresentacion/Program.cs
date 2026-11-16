using System;
using System.Windows.Forms;

namespace RentCarPresentacion
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            while (true)
            {
                Sesion.Cerrar();

                // Login: el usuario elige si entra como Cliente o como Empleado/Administrador
                using (FrmLogin login = new FrmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK) return;
                }

                // Cada rol ve una interfaz distinta
                Form principal;
                if (Sesion.Rol == Roles.Cliente)
                    principal = new FrmPortalCliente();
                else
                    principal = new FrmMenuOperacion();

                Application.Run(principal);

                // Si cerró con "Cerrar sesión" vuelve al login; si cerró la ventana con la X, termina
                if (!Sesion.CerroSesion) return;
            }
        }
    }
}
