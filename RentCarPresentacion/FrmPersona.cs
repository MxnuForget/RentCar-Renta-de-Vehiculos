using System;
using System.Windows.Forms;

public partial class FrmPersona : Form
{
    private PersonaNegocio negocio = new PersonaNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();
    private int clave1 = 0, clave2 = 0;   // llave del registro seleccionado en la tabla

    public FrmPersona()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmPersona_Load(object sender, EventArgs e)
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
            Persona x = Armar();
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
            if (clave1 <= 0) throw new Exception("Seleccione un persona de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este persona?", "Confirmar",
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
    private void dgvPersonas_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow f = dgvPersonas.Rows[e.RowIndex];
        try
        {
            clave1 = Convert.ToInt32(f.Cells["id_persona"].Value);
            txtNombres.Text = Convert.ToString(f.Cells["nombres"].Value);
            txtApPaterno.Text = Convert.ToString(f.Cells["ap_paterno"].Value);
            txtApMaterno.Text = Convert.ToString(f.Cells["ap_materno"].Value);
            txtCi.Text = Convert.ToString(f.Cells["ci"].Value);
            dtpNacimiento.Value = Convert.ToDateTime(f.Cells["fecha_nacimiento"].Value);
            txtEmail.Text = Convert.ToString(f.Cells["email"].Value);
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private Persona Armar()
    {
            return new Persona
            {
                Nombres = txtNombres.Text.Trim(),
                ApPaterno = txtApPaterno.Text.Trim(),
                ApMaterno = txtApMaterno.Text.Trim(),
                Ci = txtCi.Text.Trim(),
                FechaNacimiento = dtpNacimiento.Value,
                Email = txtEmail.Text.Trim()
            };
    }

    private void CargarCombos()
    {

    }

    private void CargarGrilla()
    {
        dgvPersonas.DataSource = negocio.Listar();
        Estilo.Columnas(dgvPersonas, "id_persona");
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
            txtNombres.Clear(); txtApPaterno.Clear(); txtApMaterno.Clear(); txtCi.Clear(); txtEmail.Clear();
            dtpNacimiento.Value = DateTime.Today.AddYears(-25);
        dgvPersonas.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
