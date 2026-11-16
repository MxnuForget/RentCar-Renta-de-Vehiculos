partial class FrmGestionEmpleados
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
            this.lblNombres = new System.Windows.Forms.Label();
            this.txtNombres = new System.Windows.Forms.TextBox();
            this.lblApPaterno = new System.Windows.Forms.Label();
            this.txtApPaterno = new System.Windows.Forms.TextBox();
            this.lblApMaterno = new System.Windows.Forms.Label();
            this.txtApMaterno = new System.Windows.Forms.TextBox();
            this.lblCi = new System.Windows.Forms.Label();
            this.txtCi = new System.Windows.Forms.TextBox();
            this.lblNacimiento = new System.Windows.Forms.Label();
            this.dtpNacimiento = new System.Windows.Forms.DateTimePicker();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblSalario = new System.Windows.Forms.Label();
            this.nudSalario = new System.Windows.Forms.NumericUpDown();
            this.lblCargo = new System.Windows.Forms.Label();
            this.cboCargo = new System.Windows.Forms.ComboBox();
            this.lblSucursal = new System.Windows.Forms.Label();
            this.cboSucursal = new System.Windows.Forms.ComboBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.cboRol = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblClave = new System.Windows.Forms.Label();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.lblClave2 = new System.Windows.Forms.Label();
            this.txtClave2 = new System.Windows.Forms.TextBox();
            this.lblRequisitos = new System.Windows.Forms.Label();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnActivar = new System.Windows.Forms.Button();
            this.dgvEmpleados = new System.Windows.Forms.DataGridView();
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSalario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados)).BeginInit();
            this.pnlTitulo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNombres
            // 
            this.lblNombres.AutoSize = true;
            this.lblNombres.Location = new System.Drawing.Point(20, 88);
            this.lblNombres.Name = "lblNombres";
            this.lblNombres.TabIndex = 0;
            this.lblNombres.Text = "Nombres:";
            // 
            // txtNombres
            // 
            this.txtNombres.Location = new System.Drawing.Point(175, 84);
            this.txtNombres.Name = "txtNombres";
            this.txtNombres.Size = new System.Drawing.Size(230, 27);
            this.txtNombres.TabIndex = 1;
            // 
            // lblApPaterno
            // 
            this.lblApPaterno.AutoSize = true;
            this.lblApPaterno.Location = new System.Drawing.Point(20, 126);
            this.lblApPaterno.Name = "lblApPaterno";
            this.lblApPaterno.TabIndex = 2;
            this.lblApPaterno.Text = "Apellido paterno:";
            // 
            // txtApPaterno
            // 
            this.txtApPaterno.Location = new System.Drawing.Point(175, 122);
            this.txtApPaterno.Name = "txtApPaterno";
            this.txtApPaterno.Size = new System.Drawing.Size(230, 27);
            this.txtApPaterno.TabIndex = 3;
            // 
            // lblApMaterno
            // 
            this.lblApMaterno.AutoSize = true;
            this.lblApMaterno.Location = new System.Drawing.Point(20, 164);
            this.lblApMaterno.Name = "lblApMaterno";
            this.lblApMaterno.TabIndex = 4;
            this.lblApMaterno.Text = "Apellido materno:";
            // 
            // txtApMaterno
            // 
            this.txtApMaterno.Location = new System.Drawing.Point(175, 160);
            this.txtApMaterno.Name = "txtApMaterno";
            this.txtApMaterno.Size = new System.Drawing.Size(230, 27);
            this.txtApMaterno.TabIndex = 5;
            // 
            // lblCi
            // 
            this.lblCi.AutoSize = true;
            this.lblCi.Location = new System.Drawing.Point(20, 202);
            this.lblCi.Name = "lblCi";
            this.lblCi.TabIndex = 6;
            this.lblCi.Text = "CI:";
            // 
            // txtCi
            // 
            this.txtCi.Location = new System.Drawing.Point(175, 198);
            this.txtCi.Name = "txtCi";
            this.txtCi.Size = new System.Drawing.Size(230, 27);
            this.txtCi.TabIndex = 7;
            // 
            // lblNacimiento
            // 
            this.lblNacimiento.AutoSize = true;
            this.lblNacimiento.Location = new System.Drawing.Point(20, 240);
            this.lblNacimiento.Name = "lblNacimiento";
            this.lblNacimiento.TabIndex = 8;
            this.lblNacimiento.Text = "Fecha nacimiento:";
            // 
            // dtpNacimiento
            // 
            this.dtpNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNacimiento.Location = new System.Drawing.Point(175, 236);
            this.dtpNacimiento.Name = "dtpNacimiento";
            this.dtpNacimiento.Size = new System.Drawing.Size(230, 27);
            this.dtpNacimiento.TabIndex = 9;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(20, 278);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Correo:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(175, 274);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(230, 27);
            this.txtEmail.TabIndex = 11;
            // 
            // lblSalario
            // 
            this.lblSalario.AutoSize = true;
            this.lblSalario.Location = new System.Drawing.Point(20, 316);
            this.lblSalario.Name = "lblSalario";
            this.lblSalario.TabIndex = 12;
            this.lblSalario.Text = "Salario:";
            // 
            // nudSalario
            // 
            this.nudSalario.DecimalPlaces = 2;
            this.nudSalario.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudSalario.Location = new System.Drawing.Point(175, 312);
            this.nudSalario.Name = "nudSalario";
            this.nudSalario.Size = new System.Drawing.Size(230, 27);
            this.nudSalario.TabIndex = 13;
            // 
            // lblCargo
            // 
            this.lblCargo.AutoSize = true;
            this.lblCargo.Location = new System.Drawing.Point(440, 88);
            this.lblCargo.Name = "lblCargo";
            this.lblCargo.TabIndex = 14;
            this.lblCargo.Text = "Cargo:";
            // 
            // cboCargo
            // 
            this.cboCargo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCargo.Location = new System.Drawing.Point(600, 84);
            this.cboCargo.Name = "cboCargo";
            this.cboCargo.Size = new System.Drawing.Size(250, 27);
            this.cboCargo.TabIndex = 15;
            // 
            // lblSucursal
            // 
            this.lblSucursal.AutoSize = true;
            this.lblSucursal.Location = new System.Drawing.Point(440, 126);
            this.lblSucursal.Name = "lblSucursal";
            this.lblSucursal.TabIndex = 16;
            this.lblSucursal.Text = "Sucursal:";
            // 
            // cboSucursal
            // 
            this.cboSucursal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSucursal.Location = new System.Drawing.Point(600, 122);
            this.cboSucursal.Name = "cboSucursal";
            this.cboSucursal.Size = new System.Drawing.Size(250, 27);
            this.cboSucursal.TabIndex = 17;
            // 
            // lblRol
            // 
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new System.Drawing.Point(440, 164);
            this.lblRol.Name = "lblRol";
            this.lblRol.TabIndex = 18;
            this.lblRol.Text = "Rol:";
            // 
            // cboRol
            // 
            this.cboRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRol.Location = new System.Drawing.Point(600, 160);
            this.cboRol.Name = "cboRol";
            this.cboRol.Size = new System.Drawing.Size(250, 27);
            this.cboRol.TabIndex = 19;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(440, 202);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.TabIndex = 20;
            this.lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(600, 198);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(250, 27);
            this.txtUsuario.TabIndex = 21;
            // 
            // lblClave
            // 
            this.lblClave.AutoSize = true;
            this.lblClave.Location = new System.Drawing.Point(440, 240);
            this.lblClave.Name = "lblClave";
            this.lblClave.TabIndex = 22;
            this.lblClave.Text = "Contraseña:";
            // 
            // txtClave
            // 
            this.txtClave.UseSystemPasswordChar = true;
            this.txtClave.Location = new System.Drawing.Point(600, 236);
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(250, 27);
            this.txtClave.TabIndex = 23;
            // 
            // lblClave2
            // 
            this.lblClave2.AutoSize = true;
            this.lblClave2.Location = new System.Drawing.Point(440, 278);
            this.lblClave2.Name = "lblClave2";
            this.lblClave2.TabIndex = 24;
            this.lblClave2.Text = "Repetir contraseña:";
            // 
            // txtClave2
            // 
            this.txtClave2.UseSystemPasswordChar = true;
            this.txtClave2.Location = new System.Drawing.Point(600, 274);
            this.txtClave2.Name = "txtClave2";
            this.txtClave2.Size = new System.Drawing.Size(250, 27);
            this.txtClave2.TabIndex = 25;
            // 
            // lblRequisitos
            // 
            this.lblRequisitos.AutoSize = true;
            this.lblRequisitos.Location = new System.Drawing.Point(440, 316);
            this.lblRequisitos.Name = "lblRequisitos";
            this.lblRequisitos.TabIndex = 26;
            this.lblRequisitos.Text = "Mínimo 8 caracteres, con letra y número";
            // 
            // btnCrear
            // 
            this.btnCrear.Location = new System.Drawing.Point(20, 375);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(170, 38);
            this.btnCrear.TabIndex = 27;
            this.btnCrear.Text = "Crear empleado";
            this.btnCrear.UseVisualStyleBackColor = true;
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(200, 375);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(130, 38);
            this.btnLimpiar.TabIndex = 28;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnActivar
            // 
            this.btnActivar.Location = new System.Drawing.Point(340, 375);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(210, 38);
            this.btnActivar.TabIndex = 29;
            this.btnActivar.Text = "Activar / Desactivar";
            this.btnActivar.UseVisualStyleBackColor = true;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            // 
            // dgvEmpleados
            // 
            this.dgvEmpleados.Location = new System.Drawing.Point(20, 430);
            this.dgvEmpleados.Name = "dgvEmpleados";
            this.dgvEmpleados.Size = new System.Drawing.Size(830, 270);
            this.dgvEmpleados.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvEmpleados.TabIndex = 30;
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(870, 64);
            this.pnlTitulo.TabIndex = 31;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(20, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Gestión de Empleados";
            // 
            // FrmGestionEmpleados
            // 
            this.ClientSize = new System.Drawing.Size(870, 720);
            this.Controls.Add(this.lblNombres);
            this.Controls.Add(this.txtNombres);
            this.Controls.Add(this.lblApPaterno);
            this.Controls.Add(this.txtApPaterno);
            this.Controls.Add(this.lblApMaterno);
            this.Controls.Add(this.txtApMaterno);
            this.Controls.Add(this.lblCi);
            this.Controls.Add(this.txtCi);
            this.Controls.Add(this.lblNacimiento);
            this.Controls.Add(this.dtpNacimiento);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblSalario);
            this.Controls.Add(this.nudSalario);
            this.Controls.Add(this.lblCargo);
            this.Controls.Add(this.cboCargo);
            this.Controls.Add(this.lblSucursal);
            this.Controls.Add(this.cboSucursal);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.cboRol);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblClave);
            this.Controls.Add(this.txtClave);
            this.Controls.Add(this.lblClave2);
            this.Controls.Add(this.txtClave2);
            this.Controls.Add(this.lblRequisitos);
            this.Controls.Add(this.btnCrear);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnActivar);
            this.Controls.Add(this.dgvEmpleados);
            this.Controls.Add(this.pnlTitulo);
            this.MinimumSize = new System.Drawing.Size(870, 720);
            this.Name = "FrmGestionEmpleados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Empleados";
            this.Load += new System.EventHandler(this.FrmGestionEmpleados_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudSalario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados)).EndInit();
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
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
    private System.Windows.Forms.Label lblSalario;
    private System.Windows.Forms.NumericUpDown nudSalario;
    private System.Windows.Forms.Label lblCargo;
    private System.Windows.Forms.ComboBox cboCargo;
    private System.Windows.Forms.Label lblSucursal;
    private System.Windows.Forms.ComboBox cboSucursal;
    private System.Windows.Forms.Label lblRol;
    private System.Windows.Forms.ComboBox cboRol;
    private System.Windows.Forms.Label lblUsuario;
    private System.Windows.Forms.TextBox txtUsuario;
    private System.Windows.Forms.Label lblClave;
    private System.Windows.Forms.TextBox txtClave;
    private System.Windows.Forms.Label lblClave2;
    private System.Windows.Forms.TextBox txtClave2;
    private System.Windows.Forms.Label lblRequisitos;
    private System.Windows.Forms.Button btnCrear;
    private System.Windows.Forms.Button btnLimpiar;
    private System.Windows.Forms.Button btnActivar;
    private System.Windows.Forms.DataGridView dgvEmpleados;
    private System.Windows.Forms.Panel pnlTitulo;
    private System.Windows.Forms.Label lblTitulo;
}
