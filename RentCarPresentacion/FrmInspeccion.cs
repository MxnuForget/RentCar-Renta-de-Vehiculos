using System;
using System.Windows.Forms;

public partial class FrmInspeccion : Form
{
    private InspeccionNegocio negocio = new InspeccionNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();
    private int clave1 = 0, clave2 = 0;   // llave del registro seleccionado en la tabla

    public FrmInspeccion()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmInspeccion_Load(object sender, EventArgs e)
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
            Inspeccion x = Armar();
            x.IdInspeccion = clave1;
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
            if (clave1 <= 0) throw new Exception("Seleccione un inspección de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este inspección?", "Confirmar",
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
    private void dgvInspecciones_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow f = dgvInspecciones.Rows[e.RowIndex];
        try
        {
            clave1 = Convert.ToInt32(f.Cells["id_inspeccion"].Value);
            cboAlquiler.SelectedValue = f.Cells["id_alquiler"].Value;
            cboTipo.SelectedItem = Convert.ToString(f.Cells["tipo_inspeccion"].Value);
            dtpFecha.Value = Convert.ToDateTime(f.Cells["fecha_inspeccion"].Value);
            txtObservaciones.Text = Convert.ToString(f.Cells["observaciones"].Value);
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private Inspeccion Armar()
    {
            return new Inspeccion
            {
                IdAlquiler = Valor(cboAlquiler),
                TipoInspeccion = cboTipo.SelectedItem as string,
                FechaInspeccion = dtpFecha.Value,
                Observaciones = txtObservaciones.Text.Trim()
            };
    }

    private void CargarCombos()
    {
            cboAlquiler.DisplayMember = "Nombre"; cboAlquiler.ValueMember = "Id"; cboAlquiler.DataSource = cat.Alquileres(); cboAlquiler.SelectedIndex = -1;
            cboTipo.Items.AddRange(cat.TiposInspeccion());
    }

    private void CargarGrilla()
    {
        dgvInspecciones.DataSource = negocio.Listar();
        Estilo.Columnas(dgvInspecciones);
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

    }

    private void Limpiar()
    {
        clave1 = 0; clave2 = 0;
            cboAlquiler.SelectedIndex = -1; cboTipo.SelectedIndex = -1; dtpFecha.Value = DateTime.Now; txtObservaciones.Clear();
        dgvInspecciones.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
