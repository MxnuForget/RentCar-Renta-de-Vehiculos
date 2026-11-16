using System;
using System.Windows.Forms;

// Solo el ADMINISTRADOR: crea cuentas de empleado y las activa o desactiva
public partial class FrmGestionEmpleados : Form
{
    private EmpleadoNegocio negocio = new EmpleadoNegocio();
    private CatalogoNegocio cat = new CatalogoNegocio();

    public FrmGestionEmpleados()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        lblRequisitos.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        dtpNacimiento.Value = DateTime.Today.AddYears(-25);
    }

    private void FrmGestionEmpleados_Load(object sender, EventArgs e)
    {
        try
        {
            cboCargo.DisplayMember = "Nombre"; cboCargo.ValueMember = "Id"; cboCargo.DataSource = cat.Cargos(); cboCargo.SelectedIndex = -1;
            cboSucursal.DisplayMember = "Nombre"; cboSucursal.ValueMember = "Id"; cboSucursal.DataSource = cat.Sucursales(); cboSucursal.SelectedIndex = -1;
            cboRol.Items.Add(Roles.Administrador);
            cboRol.Items.Add(Roles.Operador);
            cboRol.SelectedIndex = -1;
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnCrear_Click(object sender, EventArgs e)
    {
        try
        {
            EmpleadoRegistro r = new EmpleadoRegistro
            {
                Nombres = txtNombres.Text.Trim(),
                ApPaterno = txtApPaterno.Text.Trim(),
                ApMaterno = txtApMaterno.Text.Trim(),
                Ci = txtCi.Text.Trim(),
                FechaNacimiento = dtpNacimiento.Value,
                Email = txtEmail.Text.Trim(),
                Salario = nudSalario.Value,
                IdCargo = Valor(cboCargo),
                IdSucursal = Valor(cboSucursal),
                Rol = cboRol.SelectedItem as string,
                Usuario = txtUsuario.Text.Trim(),
                Clave = txtClave.Text
            };
            negocio.Crear(r, txtClave2.Text);
            Mensaje("Empleado creado correctamente.", false);
            Limpiar();
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); }

    private void btnActivar_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvEmpleados.SelectedRows.Count == 0) throw new Exception("Seleccione un empleado de la tabla.");
            DataGridViewRow f = dgvEmpleados.SelectedRows[0];
            int id = Convert.ToInt32(f.Cells["id_persona"].Value);
            bool activoAhora = Convert.ToBoolean(f.Cells["activo"].Value);
            string usuario = Convert.ToString(f.Cells["usuario"].Value);

            string pregunta = activoAhora ? "¿Desactivar la cuenta de " + usuario + "? No podrá iniciar sesión."
                                          : "¿Activar la cuenta de " + usuario + "?";
            if (MessageBox.Show(pregunta, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            negocio.CambiarActivo(id, !activoAhora);
            Mensaje(activoAhora ? "Cuenta desactivada." : "Cuenta activada.", false);
            CargarGrilla();
        }
        catch (Exception ex) { Mensaje(ex.Message, true); }
    }

    private void CargarGrilla()
    {
        dgvEmpleados.DataSource = negocio.Listar();
        Estilo.Columnas(dgvEmpleados, "id_persona");
    }

    private int Valor(ComboBox cbo)
    {
        return cbo.SelectedValue == null ? 0 : Convert.ToInt32(cbo.SelectedValue);
    }

    private void Limpiar()
    {
        txtNombres.Clear(); txtApPaterno.Clear(); txtApMaterno.Clear(); txtCi.Clear(); txtEmail.Clear();
        txtUsuario.Clear(); txtClave.Clear(); txtClave2.Clear();
        dtpNacimiento.Value = DateTime.Today.AddYears(-25);
        nudSalario.Value = 0;
        cboCargo.SelectedIndex = -1; cboSucursal.SelectedIndex = -1; cboRol.SelectedIndex = -1;
        txtNombres.Focus();
    }

    private void Mensaje(string texto, bool error)
    {
        MessageBox.Show(texto, error ? "Error" : "Éxito", MessageBoxButtons.OK,
            error ? MessageBoxIcon.Error : MessageBoxIcon.Information);
    }
}
