partial class FrmLogin
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
        this.lblUsuario = new System.Windows.Forms.Label();
        this.txtUsuario = new System.Windows.Forms.TextBox();
        this.lblClave = new System.Windows.Forms.Label();
        this.txtClave = new System.Windows.Forms.TextBox();
        this.chkMostrar = new System.Windows.Forms.CheckBox();
        this.btnIngresar = new System.Windows.Forms.Button();
        this.btnRegistrarse = new System.Windows.Forms.Button();
        this.SuspendLayout();
        //
        // lblUsuario
        //
        this.lblUsuario.AutoSize = true;
        this.lblUsuario.Name = "lblUsuario";
        this.lblUsuario.Text = "Usuario";
        //
        // txtUsuario
        //
        this.txtUsuario.Name = "txtUsuario";
        this.txtUsuario.TabIndex = 0;
        //
        // lblClave
        //
        this.lblClave.AutoSize = true;
        this.lblClave.Name = "lblClave";
        this.lblClave.Text = "Contraseña";
        //
        // txtClave
        //
        this.txtClave.Name = "txtClave";
        this.txtClave.TabIndex = 1;
        this.txtClave.UseSystemPasswordChar = true;
        //
        // chkMostrar
        //
        this.chkMostrar.AutoSize = true;
        this.chkMostrar.Name = "chkMostrar";
        this.chkMostrar.TabIndex = 2;
        this.chkMostrar.Text = "Mostrar contraseña";
        this.chkMostrar.CheckedChanged += new System.EventHandler(this.chkMostrar_CheckedChanged);
        //
        // btnIngresar
        //
        this.btnIngresar.Name = "btnIngresar";
        this.btnIngresar.TabIndex = 3;
        this.btnIngresar.Text = "Iniciar sesión";
        this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
        //
        // btnRegistrarse
        //
        this.btnRegistrarse.Name = "btnRegistrarse";
        this.btnRegistrarse.TabIndex = 4;
        this.btnRegistrarse.Text = "Crear cuenta de cliente";
        this.btnRegistrarse.Click += new System.EventHandler(this.btnRegistrarse_Click);
        //
        // FrmLogin
        //
        this.ClientSize = new System.Drawing.Size(1000, 640);
        this.Controls.Add(this.lblUsuario);
        this.Controls.Add(this.txtUsuario);
        this.Controls.Add(this.lblClave);
        this.Controls.Add(this.txtClave);
        this.Controls.Add(this.chkMostrar);
        this.Controls.Add(this.btnIngresar);
        this.Controls.Add(this.btnRegistrarse);
        this.Name = "FrmLogin";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "RentCar - Iniciar sesión";
        this.Load += new System.EventHandler(this.FrmLogin_Load);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblUsuario;
    private System.Windows.Forms.TextBox txtUsuario;
    private System.Windows.Forms.Label lblClave;
    private System.Windows.Forms.TextBox txtClave;
    private System.Windows.Forms.CheckBox chkMostrar;
    private System.Windows.Forms.Button btnIngresar;
    private System.Windows.Forms.Button btnRegistrarse;
}
