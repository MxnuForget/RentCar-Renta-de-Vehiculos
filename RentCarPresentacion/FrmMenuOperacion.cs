using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// Ventana principal del PERSONAL (Administrador u Operador).
// Menú lateral fijo + zona de contenido: cada módulo se abre DENTRO de la ventana,
// así el menú siempre queda disponible y se puede pasar de un módulo a otro sin cerrar nada.
public class FrmMenuOperacion : Form
{
    private class Modulo
    {
        public string Seccion;
        public string Clave;
        public string Texto;
        public string Descripcion;
        public Color Acento;
        public Func<Form> Crear;
    }

    private List<Modulo> modulos = new List<Modulo>();
    private List<BotonNav> botonesNav = new List<BotonNav>();
    private Panel pnlContenido = new Panel();
    private Panel pnlInicio;
    private Form actual;
    private BotonNav navInicio;

    public FrmMenuOperacion()
    {
        bool admin = Sesion.EsAdministrador;
        Text = admin ? "RentCar - Panel de administración" : "RentCar - Panel de operación";
        Font = Estilo.FuenteBase;
        BackColor = Estilo.Fondo;
        MinimumSize = new Size(Estilo.Px(1100), Estilo.Px(660));
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;

        DefinirModulos(admin);

        Panel lateral = CrearLateral();
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.BackColor = Estilo.Fondo;
        pnlInicio = CrearInicio(admin);
        pnlContenido.Controls.Add(pnlInicio);

        Controls.Add(pnlContenido);   // Fill primero
        Controls.Add(lateral);        // luego el panel Left

        Estilo.IrAReporte = IrAReporte;
        FormClosed += (s, e) => { Estilo.IrAReporte = null; };
        Marcar("inicio");
    }

    private void DefinirModulos(bool admin)
    {
        modulos.Add(new Modulo { Seccion = "OPERACIÓN", Clave = "clientes", Texto = "Clientes", Descripcion = "Registre y consulte los clientes y sus licencias.", Acento = Estilo.Azul, Crear = () => new FrmCliente() });
        modulos.Add(new Modulo { Seccion = "OPERACIÓN", Clave = "alquileres", Texto = "Gestión de alquileres", Descripcion = "Cree alquileres, fechas y estados de cada vehículo.", Acento = Estilo.Verde, Crear = () => new FrmAlquiler() });
        modulos.Add(new Modulo { Seccion = "OPERACIÓN", Clave = "pagos", Texto = "Pagos", Descripcion = "Registre los pagos de los alquileres.", Acento = Estilo.Ambar, Crear = () => new FrmPago() });
        modulos.Add(new Modulo { Seccion = "OPERACIÓN", Clave = "inspecciones", Texto = "Inspecciones", Descripcion = "Control del estado del vehículo al entregar y recibir.", Acento = Estilo.Turquesa, Crear = () => new FrmInspeccion() });
        modulos.Add(new Modulo { Seccion = "OPERACIÓN", Clave = "danios", Texto = "Detalle de daños", Descripcion = "Daños encontrados y su costo de reparación.", Acento = Estilo.Rojo, Crear = () => new FrmDetalleDanio() });
        if (admin)
        {
            modulos.Add(new Modulo { Seccion = "ADMINISTRACIÓN", Clave = "realizados", Texto = "Alquileres realizados", Descripcion = "Vista completa con filtros por estado y fechas.", Acento = Estilo.Azul, Crear = () => new FrmAdminAlquileres() });
            modulos.Add(new Modulo { Seccion = "ADMINISTRACIÓN", Clave = "empleados", Texto = "Empleados", Descripcion = "Cree cuentas de empleado y actívelas o desactívelas.", Acento = Estilo.Verde, Crear = () => new FrmGestionEmpleados() });
            modulos.Add(new Modulo { Seccion = "ADMINISTRACIÓN", Clave = "personas", Texto = "Personas", Descripcion = "Datos personales registrados en el sistema.", Acento = Estilo.Turquesa, Crear = () => new FrmPersona() });
            modulos.Add(new Modulo { Seccion = "ADMINISTRACIÓN", Clave = "metodos", Texto = "Métodos de pago", Descripcion = "Formas de pago disponibles.", Acento = Estilo.Ambar, Crear = () => new FrmMetodoPago() });
        }
        modulos.Add(new Modulo { Seccion = "ANÁLISIS", Clave = "reportes", Texto = "Reportes", Descripcion = "Cinco reportes con filtros, buscador y salida a PDF.", Acento = Estilo.Marino, Crear = () => new FrmReportes() });
    }

    // ---------- Menú lateral ----------
    private Panel CrearLateral()
    {
        Panel p = new Panel();
        p.Dock = DockStyle.Left;
        p.Width = Estilo.Px(262);
        p.BackColor = Estilo.Oscuro;

        // marca (auto + nombre)
        Panel marca = new Panel();
        marca.Dock = DockStyle.Top;
        marca.Height = Estilo.Px(96);
        marca.BackColor = Estilo.Oscuro;
        marca.Paint += (s, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Decor.Auto(g, Estilo.Px(20), Estilo.Px(24), Estilo.Px(84), Estilo.Ambar);
            using (Font f1 = new Font("Segoe UI Semibold", 17F, FontStyle.Bold))
            using (Font f2 = new Font("Segoe UI", 8.5F))
            using (SolidBrush b1 = new SolidBrush(Color.White))
            using (SolidBrush b2 = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                g.DrawString("RentCar", f1, b1, Estilo.Px(114), Estilo.Px(22));
                g.DrawString("Alquiler de vehículos", f2, b2, Estilo.Px(116), Estilo.Px(52));
            }
        };

        // tarjeta del usuario (abajo)
        Panel usuario = new Panel();
        usuario.Dock = DockStyle.Bottom;
        usuario.Height = Estilo.Px(104);
        usuario.BackColor = Estilo.Oscuro;
        usuario.Paint += (s, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            using (Pen linea = new Pen(Color.FromArgb(51, 65, 85))) g.DrawLine(linea, Estilo.Px(16), 0, usuario.Width - Estilo.Px(16), 0);
            int d = Estilo.Px(38);
            using (SolidBrush c = new SolidBrush(Estilo.Azul)) g.FillEllipse(c, Estilo.Px(20), Estilo.Px(12), d, d);
            string inicial = string.IsNullOrEmpty(Sesion.Usuario) ? "?" : Sesion.Usuario.Substring(0, 1).ToUpper();
            TextRenderer.DrawText(g, inicial, Estilo.FuenteBoton, new Rectangle(Estilo.Px(20), Estilo.Px(12), d, d), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            using (Font f1 = new Font("Segoe UI Semibold", 10.5F))
            using (Font f2 = new Font("Segoe UI", 9F))
            using (SolidBrush b1 = new SolidBrush(Color.White))
            using (SolidBrush b2 = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                g.DrawString(Sesion.Usuario, f1, b1, Estilo.Px(68), Estilo.Px(12));
                g.DrawString(Sesion.Rol, f2, b2, Estilo.Px(69), Estilo.Px(32));
            }
        };
        Button cerrar = new Button();
        cerrar.Name = "btnCerrarSesion";
        cerrar.Text = "Cerrar sesión";
        usuario.Controls.Add(cerrar);
        Estilo.Boton(cerrar, 36);
        cerrar.Size = new Size(Estilo.Px(222), Estilo.Px(36));
        cerrar.Location = new Point(Estilo.Px(20), Estilo.Px(58));
        cerrar.Click += (s, e) => CerrarSesion();

        // lista de módulos
        FlowLayoutPanel lista = new FlowLayoutPanel();
        lista.Dock = DockStyle.Fill;
        lista.FlowDirection = FlowDirection.TopDown;
        lista.WrapContents = false;
        lista.AutoScroll = true;
        lista.BackColor = Estilo.Oscuro;
        lista.Padding = new Padding(0, Estilo.Px(4), 0, 0);

        navInicio = NuevoNav("inicio", "Inicio");
        navInicio.Click += (s, e) => MostrarInicio();
        lista.Controls.Add(navInicio);

        string seccion = "";
        foreach (Modulo m in modulos)
        {
            if (m.Seccion != seccion)
            {
                seccion = m.Seccion;
                Label l = new Label();
                l.Text = seccion;
                l.AutoSize = false;
                l.Size = new Size(Estilo.Px(262), Estilo.Px(28));
                l.Margin = new Padding(0);
                l.Padding = new Padding(Estilo.Px(24), 0, 0, 0);
                l.TextAlign = ContentAlignment.BottomLeft;
                l.Font = new Font("Segoe UI Semibold", 8F);
                l.ForeColor = Color.FromArgb(100, 116, 139);
                l.BackColor = Estilo.Oscuro;
                lista.Controls.Add(l);
            }
            BotonNav nav = NuevoNav(m.Clave, m.Texto);
            Modulo copia = m;
            nav.Click += (s, e) => Abrir(copia);
            lista.Controls.Add(nav);
        }

        p.Controls.Add(lista);     // Fill primero
        p.Controls.Add(usuario);   // Bottom
        p.Controls.Add(marca);     // Top
        return p;
    }

    private BotonNav NuevoNav(string clave, string texto)
    {
        BotonNav b = new BotonNav();
        b.Tag = clave;
        b.Text = texto;
        b.Font = new Font("Segoe UI", 10.5F);
        b.Size = new Size(Estilo.Px(262), Estilo.Px(36));
        b.Margin = new Padding(0);
        botonesNav.Add(b);
        return b;
    }

    // ---------- Pantalla de inicio ----------
    private Panel CrearInicio(bool admin)
    {
        Panel p = new Panel();
        p.Dock = DockStyle.Fill;
        p.BackColor = Estilo.Fondo;

        PanelDegradado cab = Estilo.Cabecera("Bienvenido, " + Sesion.Usuario,
            admin ? "Panel de administración  ·  acceso completo al sistema" : "Panel de operación  ·  gestión diaria de alquileres");
        cab.Height = Estilo.Px(150);
        cab.AutoAncho = Estilo.Px(280);

        FlowLayoutPanel tarjetas = new FlowLayoutPanel();
        tarjetas.Dock = DockStyle.Fill;
        tarjetas.AutoScroll = true;
        tarjetas.BackColor = Estilo.Fondo;
        tarjetas.Padding = new Padding(Estilo.Px(24), Estilo.Px(22), Estilo.Px(10), Estilo.Px(10));

        foreach (Modulo m in modulos)
        {
            BotonTarjeta t = new BotonTarjeta();
            t.Text = m.Texto;
            t.Descripcion = m.Descripcion;
            t.Acento = m.Acento;
            t.Size = new Size(Estilo.Px(300), Estilo.Px(112));
            t.Margin = new Padding(0, 0, Estilo.Px(18), Estilo.Px(18));
            Modulo copia = m;
            t.Click += (s, e) => Abrir(copia);
            tarjetas.Controls.Add(t);
        }

        p.Controls.Add(tarjetas);   // Fill primero
        p.Controls.Add(cab);        // Top
        return p;
    }

    // ---------- Navegación ----------
    private void Marcar(string clave)
    {
        foreach (BotonNav b in botonesNav)
        {
            b.Activo = (string)b.Tag == clave;
            b.Invalidate();
        }
    }

    private void QuitarActual()
    {
        if (actual != null)
        {
            pnlContenido.Controls.Remove(actual);
            actual.Dispose();
            actual = null;
        }
    }

    private void MostrarInicio()
    {
        QuitarActual();
        pnlInicio.Visible = true;
        Marcar("inicio");
    }

    private void Abrir(Modulo m)
    {
        Abrir(m.Clave, m.Crear);
    }

    private void Abrir(string clave, Func<Form> crear)
    {
        try
        {
            Form nuevo = crear();
            QuitarActual();
            nuevo.TopLevel = false;
            nuevo.FormBorderStyle = FormBorderStyle.None;
            nuevo.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(nuevo);
            pnlInicio.Visible = false;
            nuevo.Show();
            nuevo.BringToFront();
            actual = nuevo;
            Marcar(clave);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "No se pudo abrir la ventana", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Botón "Ver reporte" de cada módulo: abre Reportes directamente en la pestaña que corresponde
    private void IrAReporte(int pestana)
    {
        Abrir("reportes", () =>
        {
            FrmReportes r = new FrmReportes();
            r.MostrarPestana(pestana);
            return r;
        });
    }

    private void CerrarSesion()
    {
        if (MessageBox.Show("¿Cerrar sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        Sesion.CerroSesion = true;
        Close();
    }
}
