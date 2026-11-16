partial class FrmPortalCliente
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
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvDisponibles = new System.Windows.Forms.DataGridView();
            this.btnReservar = new System.Windows.Forms.Button();
            this.lblMis = new System.Windows.Forms.Label();
            this.dgvMisReservas = new System.Windows.Forms.DataGridView();
            this.btnCancelarReserva = new System.Windows.Forms.Button();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisponibles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMisReservas)).BeginInit();
            this.pnlTitulo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Location = new System.Drawing.Point(20, 76);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido";
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(20, 118);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.TabIndex = 1;
            this.lblDesde.Text = "Desde:";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(80, 114);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(150, 27);
            this.dtpDesde.TabIndex = 2;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(250, 118);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.TabIndex = 3;
            this.lblHasta.Text = "Hasta:";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(310, 114);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(150, 27);
            this.dtpHasta.TabIndex = 4;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(480, 110);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(240, 36);
            this.btnBuscar.TabIndex = 5;
            this.btnBuscar.Text = "Buscar autos disponibles";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dgvDisponibles
            // 
            this.dgvDisponibles.Location = new System.Drawing.Point(20, 160);
            this.dgvDisponibles.Name = "dgvDisponibles";
            this.dgvDisponibles.Size = new System.Drawing.Size(920, 190);
            this.dgvDisponibles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvDisponibles.TabIndex = 6;
            // 
            // btnReservar
            // 
            this.btnReservar.Location = new System.Drawing.Point(20, 360);
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Size = new System.Drawing.Size(280, 38);
            this.btnReservar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.btnReservar.TabIndex = 7;
            this.btnReservar.Text = "Reservar el auto seleccionado";
            this.btnReservar.UseVisualStyleBackColor = true;
            this.btnReservar.Click += new System.EventHandler(this.btnReservar_Click);
            // 
            // lblMis
            // 
            this.lblMis.AutoSize = true;
            this.lblMis.Location = new System.Drawing.Point(20, 420);
            this.lblMis.Name = "lblMis";
            this.lblMis.TabIndex = 8;
            this.lblMis.Text = "Mis reservas";
            // 
            // dgvMisReservas
            // 
            this.dgvMisReservas.Location = new System.Drawing.Point(20, 450);
            this.dgvMisReservas.Name = "dgvMisReservas";
            this.dgvMisReservas.Size = new System.Drawing.Size(920, 190);
            this.dgvMisReservas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvMisReservas.TabIndex = 9;
            // 
            // btnCancelarReserva
            // 
            this.btnCancelarReserva.Location = new System.Drawing.Point(20, 655);
            this.btnCancelarReserva.Name = "btnCancelarReserva";
            this.btnCancelarReserva.Size = new System.Drawing.Size(280, 38);
            this.btnCancelarReserva.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnCancelarReserva.TabIndex = 10;
            this.btnCancelarReserva.Text = "Cancelar reserva seleccionada";
            this.btnCancelarReserva.UseVisualStyleBackColor = true;
            this.btnCancelarReserva.Click += new System.EventHandler(this.btnCancelarReserva_Click);
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCerrarSesion.Location = new System.Drawing.Point(800, 72);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(140, 34);
            this.btnCerrarSesion.TabIndex = 20;
            this.btnCerrarSesion.Text = "Cerrar sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(960, 64);
            this.pnlTitulo.TabIndex = 11;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(20, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Portal del cliente";
            // 
            // FrmPortalCliente
            // 
            this.ClientSize = new System.Drawing.Size(960, 720);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.dgvDisponibles);
            this.Controls.Add(this.btnReservar);
            this.Controls.Add(this.lblMis);
            this.Controls.Add(this.dgvMisReservas);
            this.Controls.Add(this.btnCancelarReserva);
            this.Controls.Add(this.btnCerrarSesion);
            this.Controls.Add(this.pnlTitulo);
            this.MinimumSize = new System.Drawing.Size(960, 720);
            this.Name = "FrmPortalCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Portal del cliente";
            this.Load += new System.EventHandler(this.FrmPortalCliente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisponibles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMisReservas)).EndInit();
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblBienvenida;
    private System.Windows.Forms.Label lblDesde;
    private System.Windows.Forms.DateTimePicker dtpDesde;
    private System.Windows.Forms.Label lblHasta;
    private System.Windows.Forms.DateTimePicker dtpHasta;
    private System.Windows.Forms.Button btnBuscar;
    private System.Windows.Forms.DataGridView dgvDisponibles;
    private System.Windows.Forms.Button btnReservar;
    private System.Windows.Forms.Label lblMis;
    private System.Windows.Forms.DataGridView dgvMisReservas;
    private System.Windows.Forms.Button btnCancelarReserva;
    private System.Windows.Forms.Button btnCerrarSesion;
    private System.Windows.Forms.Panel pnlTitulo;
    private System.Windows.Forms.Label lblTitulo;
}
