using System;
using System.Windows.Forms;

public partial class FrmRegistroCliente : Form
{
    private ClienteNegocio negocio = new ClienteNegocio();
    public string UsuarioCreado { get; private set; }

    public FrmRegistroCliente()
    {
        InitializeComponent();
        Estilo.Aplicar(this);
        dtpNacimiento.Value = DateTime.Today.AddYears(-25);
        dtpVencLicencia.Value = DateTime.Today.AddYears(2);
    }

    private void FrmRegistroCliente_Load(object sender, EventArgs e) { txtNombres.Focus(); }

    private void btnCrear_Click(object sender, EventArgs e)
    {
        try
        {
            ClienteRegistro r = new ClienteRegistro
            {
                Nombres = txtNombres.Text.Trim(),
                ApPaterno = txtApPaterno.Text.Trim(),
                ApMaterno = txtApMaterno.Text.Trim(),
                Ci = txtCi.Text.Trim(),
                FechaNacimiento = dtpNacimiento.Value,
                Email = txtEmail.Text.Trim(),
                Licencia = txtLicencia.Text.Trim(),
                FechaVencLicencia = dtpVencLicencia.Value,
                Usuario = txtUsuario.Text.Trim(),
                Clave = txtClave.Text
            };
            negocio.Registrar(r, txtClave2.Text);
            UsuarioCreado = r.Usuario;
            MessageBox.Show("Cuenta creada correctamente. Ya puede iniciar sesión.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtNombres_TextChanged(object sender, EventArgs e)
    {

    }

    private void lblTitulo_Click(object sender, EventArgs e)
    {

    }

    private void txtApMaterno_TextChanged(object sender, EventArgs e)
    {

    }

    private void txtClave_TextChanged(object sender, EventArgs e)
    {

    }
}
