using System;
using System.Drawing;
using System.Windows.Forms;

// Interfaz del CLIENTE: buscar autos libres, reservar, ver y cancelar sus reservas
public partial class FrmPortalCliente : Form
{
    private ReservaNegocio negocio = new ReservaNegocio();

    public FrmPortalCliente()
    {
        InitializeComponent();
        ArmarDiseno();
        dtpDesde.Value = DateTime.Today;
        dtpHasta.Value = DateTime.Today.AddDays(1);
    }

    // Diseño: cabecera, tarjeta de fechas, tabla de autos disponibles y tabla "Mis reservas"
    private void ArmarDiseno()
    {
        SuspendLayout();
        Font = Estilo.FuenteBase;
        BackColor = Estilo.Fondo;
        MinimumSize = new Size(Estilo.Px(1000), Estilo.Px(660));
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;

        Controls.Clear();   // los controles del diseñador se vuelven a colocar abajo
        pnlTitulo.Dispose();

        PanelDegradado cab = Estilo.Cabecera("Portal del cliente", "Bienvenido/a, " + Sesion.NombreCompleto + ". Elija las fechas y reserve su auto.");
        cab.Height = Estilo.Px(90);
        cab.Controls.Add(btnCerrarSesion);
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.BackColor = Color.FromArgb(51, 65, 85);
        btnCerrarSesion.ForeColor = Color.White;
        btnCerrarSesion.Font = Estilo.FuenteBoton;
        btnCerrarSesion.Cursor = Cursors.Hand;
        btnCerrarSesion.Size = new Size(Estilo.Px(150), Estilo.Px(38));
        cab.Resize += (s, e) =>
        {
            btnCerrarSesion.Location = new Point(cab.Width - cab.AutoAncho - Estilo.Px(36) - btnCerrarSesion.Width - Estilo.Px(24),
                                                 (cab.Height - btnCerrarSesion.Height) / 2);
        };

        // tarjeta de búsqueda por fechas
        FlowLayoutPanel tarjeta = Estilo.Tarjeta();
        tarjeta.Dock = DockStyle.Fill;
        tarjeta.Margin = new Padding(0);
        tarjeta.Controls.Add(Estilo.Campo(lblDesde, dtpDesde, 200));
        tarjeta.Controls.Add(Estilo.Campo(lblHasta, dtpHasta, 200));
        Estilo.Boton(btnBuscar, 42);
        btnBuscar.Margin = new Padding(0, Estilo.Px(24), 0, 0);
        tarjeta.Controls.Add(btnBuscar);

        Label titulo1 = Titulo("Autos disponibles para las fechas elegidas");
        lblMis.AutoSize = true;
        lblMis.Dock = DockStyle.Fill;
        lblMis.Margin = new Padding(0, Estilo.Px(14), 0, Estilo.Px(6));
        lblMis.ForeColor = Estilo.Oscuro;
        lblMis.Font = new Font("Segoe UI Semibold", 12F);

        TableLayoutPanel tabla = new TableLayoutPanel();
        tabla.Dock = DockStyle.Fill;
        tabla.ColumnCount = 1;
        tabla.RowCount = 7;
        tabla.BackColor = Estilo.Fondo;
        tabla.Padding = new Padding(Estilo.Px(24), Estilo.Px(18), Estilo.Px(24), Estilo.Px(14));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        tabla.Controls.Add(tarjeta, 0, 0);
        tabla.Controls.Add(titulo1, 0, 1);
        tabla.Controls.Add(CeldaTabla(dgvDisponibles), 0, 2);
        tabla.Controls.Add(FilaBoton(btnReservar), 0, 3);
        tabla.Controls.Add(lblMis, 0, 4);
        tabla.Controls.Add(CeldaTabla(dgvMisReservas), 0, 5);
        tabla.Controls.Add(FilaBoton(btnCancelarReserva), 0, 6);

        Controls.Add(tabla);   // Fill primero
        Controls.Add(cab);     // luego el panel Top
        ResumeLayout(true);
    }

    private static Label Titulo(string texto)
    {
        Label l = new Label();
        l.Text = texto;
        l.AutoSize = true;
        l.Dock = DockStyle.Fill;
        l.Margin = new Padding(0, Estilo.Px(14), 0, Estilo.Px(6));
        l.ForeColor = Estilo.Oscuro;
        l.Font = new Font("Segoe UI Semibold", 12F);
        return l;
    }

    // Tabla con marco y buscador
    private static Panel CeldaTabla(DataGridView g)
    {
        Estilo.Grilla(g);
        g.Dock = DockStyle.Fill;
        Panel marco = new Panel();
        marco.Dock = DockStyle.Fill;
        marco.BackColor = Estilo.Borde;
        marco.Padding = new Padding(1);
        marco.Controls.Add(g);

        Panel celda = new Panel();
        celda.Dock = DockStyle.Fill;
        celda.Margin = new Padding(0);
        celda.BackColor = Estilo.Fondo;
        celda.Controls.Add(marco);                 // Fill primero
        celda.Controls.Add(new BarraBusqueda(g));  // Top
        return celda;
    }

    private static FlowLayoutPanel FilaBoton(Button b)
    {
        FlowLayoutPanel f = new FlowLayoutPanel();
        f.Dock = DockStyle.Fill;
        f.AutoSize = true;
        f.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        f.BackColor = Estilo.Fondo;
        f.Margin = new Padding(0);
        f.Controls.Add(b);
        Estilo.Boton(b, 42);
        b.Margin = new Padding(0, Estilo.Px(10), 0, Estilo.Px(4));
        return f;
    }

    private void FrmPortalCliente_Load(object sender, EventArgs e)
    {
        try
        {
            CargarMisReservas();
            BuscarDisponibles();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
        try { BuscarDisponibles(); }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnReservar_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvDisponibles.SelectedRows.Count == 0) throw new Exception("Seleccione un auto de la lista.");
            DataGridViewRow f = dgvDisponibles.SelectedRows[0];
            int idVehiculo = Convert.ToInt32(f.Cells["id_vehiculo"].Value);
            string detalle = Convert.ToString(f.Cells["placa"].Value) + " - " + Convert.ToString(f.Cells["modelo"].Value);
            decimal total = Convert.ToDecimal(f.Cells["total_estimado"].Value);

            DialogResult r = MessageBox.Show(
                "¿Reservar " + detalle + "\ndel " + dtpDesde.Value.ToString("dd/MM/yyyy") + " al " + dtpHasta.Value.ToString("dd/MM/yyyy") +
                "?\n\nTotal estimado: " + total.ToString("N2"),
                "Confirmar reserva", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            negocio.Reservar(Sesion.IdCliente, idVehiculo, dtpDesde.Value, dtpHasta.Value);
            Mensaje("¡Reserva realizada correctamente!", false);
            CargarMisReservas();
            BuscarDisponibles();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnCancelarReserva_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvMisReservas.SelectedRows.Count == 0) throw new Exception("Seleccione una reserva de la tabla.");
            int id = Convert.ToInt32(dgvMisReservas.SelectedRows[0].Cells["id_alquiler"].Value);
            DialogResult r = MessageBox.Show("¿Cancelar la reserva #" + id + "?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            negocio.Cancelar(id, Sesion.IdCliente);
            Mensaje("Reserva cancelada.", false);
            CargarMisReservas();
            BuscarDisponibles();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("¿Cerrar sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        Sesion.CerroSesion = true;   // Program.cs vuelve a mostrar el login
        Close();
    }

    private void BuscarDisponibles()
    {
        dgvDisponibles.DataSource = negocio.VehiculosDisponibles(dtpDesde.Value, dtpHasta.Value);
        Estilo.Columnas(dgvDisponibles, "id_vehiculo");
    }

    private void CargarMisReservas()
    {
        dgvMisReservas.DataSource = negocio.MisReservas(Sesion.IdCliente);
        Estilo.Columnas(dgvMisReservas);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
