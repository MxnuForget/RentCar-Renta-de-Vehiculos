using System;
using System.Windows.Forms;

public partial class FrmDetalleDanio : Form
{
    private DetalleDanioNegocio negocio = new DetalleDanioNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();
    private int clave1 = 0, clave2 = 0;   // llave del registro seleccionado en la tabla

    public FrmDetalleDanio()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmDetalleDanio_Load(object sender, EventArgs e)
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
            DetalleDanio x = Armar();
            x.IdInspeccion = clave1; x.NumItem = clave2;
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
            if (clave1 <= 0) throw new Exception("Seleccione un daño de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este daño?", "Confirmar",
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
    private void dgvDetalleDanios_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow f = dgvDetalleDanios.Rows[e.RowIndex];
        try
        {
            clave1 = Convert.ToInt32(f.Cells["id_inspeccion"].Value);
            clave2 = Convert.ToInt32(f.Cells["num_item"].Value);
            cboInspeccion.SelectedValue = clave1;
            txtDescripcion.Text = Convert.ToString(f.Cells["descripcion_danio"].Value);
            nudCosto.Value = Convert.ToDecimal(f.Cells["costo_reparacion"].Value);
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private DetalleDanio Armar()
    {
            return new DetalleDanio
            {
                IdInspeccion = Valor(cboInspeccion),
                DescripcionDanio = txtDescripcion.Text.Trim(),
                CostoReparacion = nudCosto.Value
            };
    }

    private void CargarCombos()
    {
            cboInspeccion.DisplayMember = "Nombre"; cboInspeccion.ValueMember = "Id"; cboInspeccion.DataSource = cat.Inspecciones(); cboInspeccion.SelectedIndex = -1;
    }

    private void CargarGrilla()
    {
        dgvDetalleDanios.DataSource = negocio.Listar();
        Estilo.Columnas(dgvDetalleDanios);
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
        cboInspeccion.Enabled = !editando;
    }

    private void Limpiar()
    {
        clave1 = 0; clave2 = 0;
            cboInspeccion.SelectedIndex = -1; txtDescripcion.Clear(); nudCosto.Value = 0;
        dgvDetalleDanios.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
