partial class FrmRegistroCliente
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblNombres = new System.Windows.Forms.Label();
        txtNombres = new System.Windows.Forms.TextBox();
        lblApPaterno = new System.Windows.Forms.Label();
        txtApPaterno = new System.Windows.Forms.TextBox();
        lblApMaterno = new System.Windows.Forms.Label();
        txtApMaterno = new System.Windows.Forms.TextBox();
        lblCi = new System.Windows.Forms.Label();
        txtCi = new System.Windows.Forms.TextBox();
        lblNacimiento = new System.Windows.Forms.Label();
        dtpNacimiento = new System.Windows.Forms.DateTimePicker();
        lblEmail = new System.Windows.Forms.Label();
        txtEmail = new System.Windows.Forms.TextBox();
        lblLicencia = new System.Windows.Forms.Label();
        txtLicencia = new System.Windows.Forms.TextBox();
        lblVencLicencia = new System.Windows.Forms.Label();
        dtpVencLicencia = new System.Windows.Forms.DateTimePicker();
        lblUsuario = new System.Windows.Forms.Label();
        txtUsuario = new System.Windows.Forms.TextBox();
        lblClave = new System.Windows.Forms.Label();
        txtClave = new System.Windows.Forms.TextBox();
        lblClave2 = new System.Windows.Forms.Label();
        txtClave2 = new System.Windows.Forms.TextBox();
        btnCrear = new System.Windows.Forms.Button();
        btnCancelar = new System.Windows.Forms.Button();
        pnlTitulo = new System.Windows.Forms.Panel();
        lblTitulo = new System.Windows.Forms.Label();
        pnlTitulo.SuspendLayout();
        SuspendLayout();
        // 
        // lblNombres
        // 
        lblNombres.AutoSize = true;
        lblNombres.Location = new System.Drawing.Point(20, 88);
        lblNombres.Name = "lblNombres";
        lblNombres.Size = new System.Drawing.Size(117, 32);
        lblNombres.TabIndex = 0;
        lblNombres.Text = "Nombres:";
        // 
        // txtNombres
        // 
        txtNombres.Location = new System.Drawing.Point(210, 84);
        txtNombres.Name = "txtNombres";
        txtNombres.Size = new System.Drawing.Size(320, 39);
        txtNombres.TabIndex = 1;
        txtNombres.TextChanged += txtNombres_TextChanged;
        // 
        // lblApPaterno
        // 
        lblApPaterno.AutoSize = true;
        lblApPaterno.Location = new System.Drawing.Point(20, 126);
        lblApPaterno.Name = "lblApPaterno";
        lblApPaterno.Size = new System.Drawing.Size(197, 32);
        lblApPaterno.TabIndex = 2;
        lblApPaterno.Text = "Apellido paterno:";
        // 
        // txtApPaterno
        // 
        txtApPaterno.Location = new System.Drawing.Point(210, 122);
        txtApPaterno.Name = "txtApPaterno";
        txtApPaterno.Size = new System.Drawing.Size(320, 39);
        txtApPaterno.TabIndex = 3;
        // 
        // lblApMaterno
        // 
        lblApMaterno.AutoSize = true;
        lblApMaterno.Location = new System.Drawing.Point(20, 164);
        lblApMaterno.Name = "lblApMaterno";
        lblApMaterno.Size = new System.Drawing.Size(204, 32);
        lblApMaterno.TabIndex = 4;
        lblApMaterno.Text = "Apellido materno:";
        // 
        // txtApMaterno
        // 
        txtApMaterno.Location = new System.Drawing.Point(210, 160);
        txtApMaterno.Name = "txtApMaterno";
        txtApMaterno.Size = new System.Drawing.Size(320, 39);
        txtApMaterno.TabIndex = 5;
        txtApMaterno.TextChanged += txtApMaterno_TextChanged;
        // 
        // lblCi
        // 
        lblCi.AutoSize = true;
        lblCi.Location = new System.Drawing.Point(20, 202);
        lblCi.Name = "lblCi";
        lblCi.Size = new System.Drawing.Size(40, 32);
        lblCi.TabIndex = 6;
        lblCi.Text = "CI:";
        // 
        // txtCi
        // 
        txtCi.Location = new System.Drawing.Point(210, 198);
        txtCi.Name = "txtCi";
        txtCi.Size = new System.Drawing.Size(320, 39);
        txtCi.TabIndex = 7;
        // 
        // lblNacimiento
        // 
        lblNacimiento.AutoSize = true;
        lblNacimiento.Location = new System.Drawing.Point(20, 240);
        lblNacimiento.Name = "lblNacimiento";
        lblNacimiento.Size = new System.Drawing.Size(241, 32);
        lblNacimiento.TabIndex = 8;
        lblNacimiento.Text = "Fecha de nacimiento:";
        // 
        // dtpNacimiento
        // 
        dtpNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
        dtpNacimiento.Location = new System.Drawing.Point(210, 236);
        dtpNacimiento.Name = "dtpNacimiento";
        dtpNacimiento.Size = new System.Drawing.Size(320, 39);
        dtpNacimiento.TabIndex = 9;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new System.Drawing.Point(20, 278);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new System.Drawing.Size(216, 32);
        lblEmail.TabIndex = 10;
        lblEmail.Text = "Correo electrónico:";
        // 
        // txtEmail
        // 
        txtEmail.Location = new System.Drawing.Point(210, 274);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new System.Drawing.Size(320, 39);
        txtEmail.TabIndex = 11;
        // 
        // lblLicencia
        // 
        lblLicencia.AutoSize = true;
        lblLicencia.Location = new System.Drawing.Point(20, 316);
        lblLicencia.Name = "lblLicencia";
        lblLicencia.Size = new System.Drawing.Size(166, 32);
        lblLicencia.TabIndex = 12;
        lblLicencia.Text = "N° de licencia:";
        // 
        // txtLicencia
        // 
        txtLicencia.Location = new System.Drawing.Point(210, 312);
        txtLicencia.Name = "txtLicencia";
        txtLicencia.Size = new System.Drawing.Size(320, 39);
        txtLicencia.TabIndex = 13;
        // 
        // lblVencLicencia
        // 
        lblVencLicencia.AutoSize = true;
        lblVencLicencia.Location = new System.Drawing.Point(20, 354);
        lblVencLicencia.Name = "lblVencLicencia";
        lblVencLicencia.Size = new System.Drawing.Size(238, 32);
        lblVencLicencia.TabIndex = 14;
        lblVencLicencia.Text = "Vencimiento licencia:";
        // 
        // dtpVencLicencia
        // 
        dtpVencLicencia.Format = System.Windows.Forms.DateTimePickerFormat.Short;
        dtpVencLicencia.Location = new System.Drawing.Point(210, 350);
        dtpVencLicencia.Name = "dtpVencLicencia";
        dtpVencLicencia.Size = new System.Drawing.Size(320, 39);
        dtpVencLicencia.TabIndex = 15;
        // 
        // lblUsuario
        // 
        lblUsuario.AutoSize = true;
        lblUsuario.Location = new System.Drawing.Point(20, 392);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new System.Drawing.Size(99, 32);
        lblUsuario.TabIndex = 16;
        lblUsuario.Text = "Usuario:";
        // 
        // txtUsuario
        // 
        txtUsuario.Location = new System.Drawing.Point(210, 388);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new System.Drawing.Size(320, 39);
        txtUsuario.TabIndex = 17;
        // 
        // lblClave
        // 
        lblClave.AutoSize = true;
        lblClave.Location = new System.Drawing.Point(20, 430);
        lblClave.Name = "lblClave";
        lblClave.Size = new System.Drawing.Size(139, 32);
        lblClave.TabIndex = 18;
        lblClave.Text = "Contraseña:";
        // 
        // txtClave
        // 
        txtClave.Location = new System.Drawing.Point(210, 426);
        txtClave.Name = "txtClave";
        txtClave.Size = new System.Drawing.Size(320, 39);
        txtClave.TabIndex = 19;
        txtClave.UseSystemPasswordChar = true;
        txtClave.TextChanged += txtClave_TextChanged;
        // 
        // lblClave2
        // 
        lblClave2.AutoSize = true;
        lblClave2.Location = new System.Drawing.Point(20, 468);
        lblClave2.Name = "lblClave2";
        lblClave2.Size = new System.Drawing.Size(217, 32);
        lblClave2.TabIndex = 20;
        lblClave2.Text = "Repetir contraseña:";
        // 
        // txtClave2
        // 
        txtClave2.Location = new System.Drawing.Point(210, 464);
        txtClave2.Name = "txtClave2";
        txtClave2.Size = new System.Drawing.Size(320, 39);
        txtClave2.TabIndex = 21;
        txtClave2.UseSystemPasswordChar = true;
        // 
        // btnCrear
        // 
        btnCrear.Location = new System.Drawing.Point(20, 517);
        btnCrear.Name = "btnCrear";
        btnCrear.Size = new System.Drawing.Size(250, 40);
        btnCrear.TabIndex = 22;
        btnCrear.Text = "Crear cuenta";
        btnCrear.UseVisualStyleBackColor = true;
        btnCrear.Click += btnCrear_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Location = new System.Drawing.Point(290, 517);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new System.Drawing.Size(240, 40);
        btnCancelar.TabIndex = 23;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // pnlTitulo
        // 
        pnlTitulo.Controls.Add(lblTitulo);
        pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
        pnlTitulo.Location = new System.Drawing.Point(0, 0);
        pnlTitulo.Name = "pnlTitulo";
        pnlTitulo.Size = new System.Drawing.Size(560, 64);
        pnlTitulo.TabIndex = 24;
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Location = new System.Drawing.Point(20, 16);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new System.Drawing.Size(261, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Crear cuenta de cliente";
        lblTitulo.Click += lblTitulo_Click;
        // 
        // FrmRegistroCliente
        // 
        ClientSize = new System.Drawing.Size(560, 577);
        Controls.Add(lblNombres);
        Controls.Add(txtNombres);
        Controls.Add(lblApPaterno);
        Controls.Add(txtApPaterno);
        Controls.Add(lblApMaterno);
        Controls.Add(txtApMaterno);
        Controls.Add(lblCi);
        Controls.Add(txtCi);
        Controls.Add(lblNacimiento);
        Controls.Add(dtpNacimiento);
        Controls.Add(lblEmail);
        Controls.Add(txtEmail);
        Controls.Add(lblLicencia);
        Controls.Add(txtLicencia);
        Controls.Add(lblVencLicencia);
        Controls.Add(dtpVencLicencia);
        Controls.Add(lblUsuario);
        Controls.Add(txtUsuario);
        Controls.Add(lblClave);
        Controls.Add(txtClave);
        Controls.Add(lblClave2);
        Controls.Add(txtClave2);
        Controls.Add(btnCrear);
        Controls.Add(btnCancelar);
        Controls.Add(pnlTitulo);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmRegistroCliente";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Crear cuenta de cliente";
        Load += FrmRegistroCliente_Load;
        pnlTitulo.ResumeLayout(false);
        pnlTitulo.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblNombres;
    private System.Windows.Forms.TextBox txtNombres;
    private System.Windows.Forms.Label lblApPaterno;
    private System.Windows.Forms.TextBox txtApPaterno;
    private System.Windows.Forms.Label lblApMaterno;
    private System.Windows.Forms.TextBox txtApMaterno;
    private System.Windows.Forms.Label lblCi;
    private System.Windows.Forms.TextBox txtCi;
    private System.Windows.Forms.Label lblNacimiento;
    private System.Windows.Forms.DateTimePicker dtpNacimiento;
    private System.Windows.Forms.Label lblEmail;
    private System.Windows.Forms.TextBox txtEmail;
    private System.Windows.Forms.Label lblLicencia;
    private System.Windows.Forms.TextBox txtLicencia;
    private System.Windows.Forms.Label lblVencLicencia;
    private System.Windows.Forms.DateTimePicker dtpVencLicencia;
    private System.Windows.Forms.Label lblUsuario;
    private System.Windows.Forms.TextBox txtUsuario;
    private System.Windows.Forms.Label lblClave;
    private System.Windows.Forms.TextBox txtClave;
    private System.Windows.Forms.Label lblClave2;
    private System.Windows.Forms.TextBox txtClave2;
    private System.Windows.Forms.Button btnCrear;
    private System.Windows.Forms.Button btnCancelar;
    private System.Windows.Forms.Panel pnlTitulo;
    private System.Windows.Forms.Label lblTitulo;
}
