using System;
using System.Windows.Forms;

public partial class FrmPago : Form
{
    private PagoNegocio negocio = new PagoNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();
    private int clave1 = 0, clave2 = 0;   // llave del registro seleccionado en la tabla

    public FrmPago()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmPago_Load(object sender, EventArgs e)
    {
        try
        {
            CargarCombos();
            CargarGrilla();
        }
        catch (Exception ex)
        {
            Mensaje(ex.Message, true);
        }
    }

    // ---------- Botones ----------
    private void btnRegistrar_Click(object sender, EventArgs e)
    {
        try
        {
            negocio.Registrar(Armar());
            Mensaje("Registro guardado correctamente.", false);
            Limpiar();
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            Pago x = Armar();
            x.IdAlquiler = clave1; x.NumPago = clave2;
            negocio.Modificar(x);
            Mensaje("Registro actualizado correctamente.", false);
            Limpiar();
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (clave1 <= 0) throw new Exception("Seleccione un pago de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este pago?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;
            negocio.Eliminar(clave1, clave2);
            Mensaje("Registro eliminado correctamente.", false);
            Limpiar();
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); }

    private void btnListar_Click(object sender, EventArgs e)
    {
        try { CargarGrilla(); }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // Al hacer clic en una fila de la tabla, sus datos pasan a los controles para modificar o eliminar
    private void dgvPagos_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow f = dgvPagos.Rows[e.RowIndex];
        try
        {
            clave1 = Convert.ToInt32(f.Cells["id_alquiler"].Value);
            clave2 = Convert.ToInt32(f.Cells["num_pago"].Value);
            cboAlquiler.SelectedValue = clave1;
            dtpFecha.Value = Convert.ToDateTime(f.Cells["fecha_pago"].Value);
            nudMonto.Value = Convert.ToDecimal(f.Cells["monto"].Value);
            cboMetodoPago.SelectedValue = f.Cells["id_metodo_pago"].Value;
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private Pago Armar()
    {
            return new Pago
            {
                IdAlquiler = Valor(cboAlquiler),
                FechaPago = dtpFecha.Value,
                Monto = nudMonto.Value,
                IdMetodoPago = Valor(cboMetodoPago)
            };
    }

    private void CargarCombos()
    {
            cboAlquiler.DisplayMember = "Nombre"; cboAlquiler.ValueMember = "Id"; cboAlquiler.DataSource = cat.Alquileres(); cboAlquiler.SelectedIndex = -1;
            cboMetodoPago.DisplayMember = "Nombre"; cboMetodoPago.ValueMember = "Id"; cboMetodoPago.DataSource = cat.MetodosPago(); cboMetodoPago.SelectedIndex = -1;
    }

    private void CargarGrilla()
    {
        dgvPagos.DataSource = negocio.Listar();
        Estilo.Columnas(dgvPagos, "id_metodo_pago");
    }

    private int Valor(ComboBox cbo)
    {
        return cbo.SelectedValue == null ? 0 : Convert.ToInt32(cbo.SelectedValue);
    }

    private void EstadoEdicion(bool editando)
    {
        btnRegistrar.Enabled = !editando;
        btnModificar.Enabled = editando;
        btnEliminar.Enabled = editando;
        cboAlquiler.Enabled = !editando;
    }

    private void Limpiar()
    {
        clave1 = 0; clave2 = 0;
            cboAlquiler.SelectedIndex = -1; cboMetodoPago.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now; nudMonto.Value = 0;
        dgvPagos.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
