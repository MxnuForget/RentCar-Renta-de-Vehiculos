using System;
using System.Windows.Forms;

public partial class FrmAlquiler : Form
{
    private AlquilerNegocio negocio = new AlquilerNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();
    private int clave1 = 0, clave2 = 0;   // llave del registro seleccionado en la tabla

    public FrmAlquiler()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        EstadoEdicion(false);
    }

    private void FrmAlquiler_Load(object sender, EventArgs e)
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
            Alquiler x = Armar();
            x.IdAlquiler = clave1;
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
            if (clave1 <= 0) throw new Exception("Seleccione un alquiler de la tabla.");
            DialogResult r = MessageBox.Show("¿Está seguro de eliminar este alquiler?", "Confirmar",
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
    private void dgvAlquileres_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow f = dgvAlquileres.Rows[e.RowIndex];
        try
        {
            clave1 = Convert.ToInt32(f.Cells["id_alquiler"].Value);
            cboCliente.SelectedValue = f.Cells["id_cliente"].Value;
            cboEmpleado.SelectedValue = f.Cells["id_empleado"].Value;
            cboVehiculo.SelectedValue = f.Cells["id_vehiculo"].Value;
            dtpInicio.Value = Convert.ToDateTime(f.Cells["fecha_inicio"].Value);
            dtpFin.Value = Convert.ToDateTime(f.Cells["fecha_fin_pactada"].Value);
            object dev = f.Cells["fecha_devolucion_real"].Value;
            if (dev == null || dev == DBNull.Value) dtpDevolucion.Checked = false;
            else { dtpDevolucion.Value = Convert.ToDateTime(dev); dtpDevolucion.Checked = true; }
            cboEstado.SelectedItem = Convert.ToString(f.Cells["estado_alquiler"].Value);
            EstadoEdicion(true);
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    // ---------- Apoyo ----------
    private Alquiler Armar()
    {
            return new Alquiler
            {
                IdCliente = Valor(cboCliente),
                IdEmpleado = Valor(cboEmpleado),
                IdVehiculo = Valor(cboVehiculo),
                FechaInicio = dtpInicio.Value,
                FechaFinPactada = dtpFin.Value,
                FechaDevolucionReal = dtpDevolucion.Checked ? (DateTime?)dtpDevolucion.Value : null,
                EstadoAlquiler = cboEstado.SelectedItem as string
            };
    }

    private void CargarCombos()
    {
            cboCliente.DisplayMember = "Nombre"; cboCliente.ValueMember = "Id"; cboCliente.DataSource = cat.Clientes(); cboCliente.SelectedIndex = -1;
            cboEmpleado.DisplayMember = "Nombre"; cboEmpleado.ValueMember = "Id"; cboEmpleado.DataSource = cat.Empleados(); cboEmpleado.SelectedIndex = -1;
            cboVehiculo.DisplayMember = "Nombre"; cboVehiculo.ValueMember = "Id"; cboVehiculo.DataSource = cat.Vehiculos(); cboVehiculo.SelectedIndex = -1;
            cboEstado.Items.AddRange(cat.EstadosAlquiler());
    }

    private void CargarGrilla()
    {
        dgvAlquileres.DataSource = negocio.Listar();
        Estilo.Columnas(dgvAlquileres, "id_cliente", "id_empleado", "id_vehiculo");
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
            cboCliente.SelectedIndex = -1; cboEmpleado.SelectedIndex = -1; cboVehiculo.SelectedIndex = -1;
            dtpInicio.Value = DateTime.Now; dtpFin.Value = DateTime.Now;
            dtpDevolucion.Checked = false; cboEstado.SelectedIndex = -1;
        dgvAlquileres.ClearSelection();
        EstadoEdicion(false);
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
