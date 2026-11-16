using System;
using System.Data;
using System.Windows.Forms;

// Interfaz del ADMINISTRADOR: ve todos los alquileres, filtra y cambia su estado
public partial class FrmAdminAlquileres : Form
{
    private AlquilerAdminNegocio negocio = new AlquilerAdminNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();

    public FrmAdminAlquileres()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
    }

    private void FrmAdminAlquileres_Load(object sender, EventArgs e)
    {
        try
        {
            cboEstado.Items.Add("(Todos)");
            cboEstado.Items.AddRange(cat.EstadosAlquiler());
            cboEstado.SelectedIndex = 0;
            Filtrar();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnFiltrar_Click(object sender, EventArgs e)
    {
        try { Filtrar(); }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        try
        {
            cboEstado.SelectedIndex = 0;
            dtpDesde.Checked = false;
            dtpHasta.Checked = false;
            txtBuscar.Clear();
            Filtrar();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnFinalizar_Click(object sender, EventArgs e)
    {
        try
        {
            int id = IdSeleccionado();
            if (MessageBox.Show("¿Marcar el alquiler #" + id + " como finalizado (devuelto hoy)?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            negocio.Finalizar(id);
            Filtrar();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnCancelarAlquiler_Click(object sender, EventArgs e)
    {
        try
        {
            int id = IdSeleccionado();
            if (MessageBox.Show("¿Cancelar el alquiler #" + id + "?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            negocio.Cancelar(id);
            Filtrar();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private int IdSeleccionado()
    {
        if (dgvAlquileres.SelectedRows.Count == 0) throw new Exception("Seleccione un alquiler de la tabla.");
        return Convert.ToInt32(dgvAlquileres.SelectedRows[0].Cells["id_alquiler"].Value);
    }

    private void Filtrar()
    {
        string estado = cboEstado.SelectedIndex > 0 ? cboEstado.SelectedItem as string : null;
        DateTime? desde = dtpDesde.Checked ? (DateTime?)dtpDesde.Value.Date : null;
        DateTime? hasta = dtpHasta.Checked ? (DateTime?)dtpHasta.Value.Date : null;
        string texto = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim();

        DataTable dt = negocio.Filtrar(estado, desde, hasta, texto);
        dgvAlquileres.DataSource = dt;
        Estilo.Columnas(dgvAlquileres);

        decimal total = 0;
        foreach (DataRow r in dt.Rows) total += Convert.ToDecimal(r["total_estimado"]);
        lblResumen.Text = dt.Rows.Count + " alquiler(es) - Total estimado: " + total.ToString("N2");
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
