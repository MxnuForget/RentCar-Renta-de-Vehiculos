using System;
using System.Windows.Forms;

public partial class FrmCliente : Form
{
    private ClienteNegocio negocio = new ClienteNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();
    private int clave1 = 0, clave2 = 0;   // llave del registro seleccionado en la tabla

    public FrmCliente()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmCliente_Load(object sender, EventArgs e)
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
            Cliente x = Armar();
            x.IdPersona = clave1;
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
            if (clave1 <= 0) throw new Exception("Seleccione un cliente de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este cliente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;
            negocio.Eliminar(clave1);
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
    private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow f = dgvClientes.Rows[e.RowIndex];
        try
        {
            clave1 = Convert.ToInt32(f.Cells["id_persona"].Value);
            cboPersona.SelectedValue = clave1;
            txtLicencia.Text = Convert.ToString(f.Cells["licencia_conducir"].Value);
            dtpVencLicencia.Value = Convert.ToDateTime(f.Cells["fecha_venc_licencia"].Value);
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private Cliente Armar()
    {
            return new Cliente
            {
                IdPersona = Valor(cboPersona),
                LicenciaConducir = txtLicencia.Text.Trim(),
                FechaVencLicencia = dtpVencLicencia.Value
            };
    }

    private void CargarCombos()
    {
            cboPersona.DisplayMember = "Nombre"; cboPersona.ValueMember = "Id"; cboPersona.DataSource = cat.Personas(); cboPersona.SelectedIndex = -1;
    }

    private void CargarGrilla()
    {
        dgvClientes.DataSource = negocio.Listar();
        Estilo.Columnas(dgvClientes, "id_persona");
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
        cboPersona.Enabled = !editando;
    }

    private void Limpiar()
    {
        clave1 = 0; clave2 = 0;
            cboPersona.SelectedIndex = -1; txtLicencia.Clear(); dtpVencLicencia.Value = DateTime.Today.AddYears(2);
        dgvClientes.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
