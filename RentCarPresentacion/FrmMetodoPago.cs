using System;
using System.Windows.Forms;

// Formulario: solo muestra datos y llama a la capa de negocio (sin SQL)
public partial class FrmMetodoPago : Form
{
    private MetodoPagoNegocio negocio = new MetodoPagoNegocio();
    private int idSeleccionado = 0;   // id del registro elegido en la tabla

    public FrmMetodoPago()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmMetodoPago_Load(object sender, EventArgs e)
    {
        try { CargarGrilla(); }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Botones ----------
    private void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            MetodoPago m = new MetodoPago { IdMetodoPago = idSeleccionado, NombreMetodo = txtNombre.Text };
            negocio.Modificar(m);
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
            if (idSeleccionado <= 0) throw new Exception("Seleccione un método de pago de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este método de pago?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            negocio.Eliminar(idSeleccionado);
            Mensaje("Registro eliminado correctamente.", false);
            Limpiar();
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); }

    // Al hacer clic en una fila, sus datos pasan al cuadro de texto para modificar o eliminar
    private void dgvMetodos_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        try
        {
            DataGridViewRow f = dgvMetodos.Rows[e.RowIndex];
            idSeleccionado = Convert.ToInt32(f.Cells["id_metodo_pago"].Value);
            txtNombre.Text = Convert.ToString(f.Cells["nombre_metodo"].Value);
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private void CargarGrilla()
    {
        dgvMetodos.DataSource = negocio.Listar();
        Estilo.Columnas(dgvMetodos, "id_metodo_pago");
    }

    private void EstadoEdicion(bool editando)
    {
        btnModificar.Enabled = editando;
        btnEliminar.Enabled = editando;
        txtNombre.Enabled = editando;
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        dgvMetodos.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
