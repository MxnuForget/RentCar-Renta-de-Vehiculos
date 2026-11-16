using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// Inicio de sesión ÚNICO: ya no se elige "Cliente" o "Empleado".
// El sistema revisa el usuario y entra con el rol que corresponda (Administrador, Operador o Cliente).
public partial class FrmLogin : Form
{
    private AutenticacionNegocio auth = new AutenticacionNegocio();
    private int intentos = 0;
    private PanelDegradado izquierda;
    private Panel derecha;
    private Panel formulario;

    public FrmLogin()
    {
        InitializeComponent();
        ArmarDiseno();
        AcceptButton = btnIngresar;   // Enter = Iniciar sesión
    }

    private void FrmLogin_Load(object sender, EventArgs e) { txtUsuario.Focus(); }

    private void chkMostrar_CheckedChanged(object sender, EventArgs e)
    {
        txtClave.UseSystemPasswordChar = !chkMostrar.Checked;
    }

    private void btnIngresar_Click(object sender, EventArgs e)
    {
        try
        {
            if (auth.IniciarSesion(txtUsuario.Text.Trim(), txtClave.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            intentos++;
            txtClave.Clear();
            if (intentos >= 3)
            {
                MessageBox.Show("Demasiados intentos fallidos. El sistema se cerrará.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos. Intento " + intentos + " de 3.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnRegistrarse_Click(object sender, EventArgs e)
    {
        using (FrmRegistroCliente f = new FrmRegistroCliente())
        {
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtUsuario.Text = f.UsuarioCreado;
                txtClave.Clear();
                txtClave.Focus();
            }
        }
    }

    // ---------------------------------------------------------------
    //  Diseño: panel izquierdo de marca (con auto) y panel derecho con el formulario
    // ---------------------------------------------------------------
    private void ArmarDiseno()
    {
        SuspendLayout();
        Font = Estilo.FuenteBase;
        BackColor = Color.White;
        Estilo.TamanoEstandar(this, 1000, 640, true);
        Controls.Clear();   // los controles siguen existiendo: se vuelven a colocar abajo

        // ----- panel izquierdo -----
        izquierda = new PanelDegradado();
        izquierda.Dock = DockStyle.Left;
        izquierda.Width = ClientSize.Width * 55 / 100;
        izquierda.MostrarAuto = false;
        izquierda.Color1 = Color.FromArgb(8, 12, 26);
        izquierda.Color2 = Estilo.Marino;
        izquierda.Direccion = LinearGradientMode.ForwardDiagonal;
        izquierda.Paint += PintarMarca;

        // ----- panel derecho -----
        derecha = new Panel();
        derecha.Dock = DockStyle.Fill;
        derecha.BackColor = Color.White;

        Label pie = new Label();
        pie.Dock = DockStyle.Bottom;
        pie.Height = Estilo.Px(40);
        pie.TextAlign = ContentAlignment.MiddleCenter;
        pie.Font = Estilo.FuenteNota;
        pie.ForeColor = Estilo.Suave;
        pie.Text = "RentCar  ·  Universidad Privada Franz Tamayo";

        formulario = new Panel();
        formulario.BackColor = Color.White;
        int w = Estilo.Px(380);
        int y = 0;

        Label bienvenida = new Label();
        bienvenida.AutoSize = true;
        bienvenida.Text = "Bienvenido";
        bienvenida.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
        bienvenida.ForeColor = Estilo.Oscuro;
        bienvenida.Location = new Point(0, y);
        y += Estilo.Px(52);

        Label sub = new Label();
        sub.AutoSize = true;
        sub.Text = "Ingrese con su usuario y contraseña";
        sub.Font = new Font("Segoe UI", 10.5F);
        sub.ForeColor = Estilo.Suave;
        sub.Location = new Point(0, y);
        y += Estilo.Px(52);

        lblUsuario.Font = Estilo.FuenteEtiqueta;
        lblUsuario.ForeColor = Estilo.Texto;
        lblUsuario.Location = new Point(0, y);
        y += Estilo.Px(24);

        txtUsuario.Font = new Font("Segoe UI", 13F);
        txtUsuario.BorderStyle = BorderStyle.FixedSingle;
        txtUsuario.Width = w;
        txtUsuario.Location = new Point(0, y);
        y += txtUsuario.PreferredSize.Height + Estilo.Px(18);

        lblClave.Font = Estilo.FuenteEtiqueta;
        lblClave.ForeColor = Estilo.Texto;
        lblClave.Location = new Point(0, y);
        y += Estilo.Px(24);

        txtClave.Font = new Font("Segoe UI", 13F);
        txtClave.BorderStyle = BorderStyle.FixedSingle;
        txtClave.Width = w;
        txtClave.Location = new Point(0, y);
        y += txtClave.PreferredSize.Height + Estilo.Px(10);

        chkMostrar.Font = Estilo.FuenteNota;
        chkMostrar.ForeColor = Estilo.Suave;
        chkMostrar.Location = new Point(0, y);
        y += Estilo.Px(40);

        Estilo.Boton(btnIngresar, 48);
        btnIngresar.Size = new Size(w, Estilo.Px(48));
        btnIngresar.Location = new Point(0, y);
        y += Estilo.Px(48) + Estilo.Px(26);

        Label separador = new Label();
        separador.AutoSize = false;
        separador.Size = new Size(w, Estilo.Px(22));
        separador.Location = new Point(0, y);
        separador.Text = "¿Aún no tiene cuenta?";
        separador.TextAlign = ContentAlignment.MiddleCenter;
        separador.Font = Estilo.FuenteNota;
        separador.ForeColor = Estilo.Suave;
        y += Estilo.Px(30);

        Estilo.Boton(btnRegistrarse, 44);
        btnRegistrarse.Size = new Size(w, Estilo.Px(44));
        btnRegistrarse.Location = new Point(0, y);
        y += Estilo.Px(44);

        formulario.Size = new Size(w, y);
        formulario.Controls.Add(bienvenida);
        formulario.Controls.Add(sub);
        formulario.Controls.Add(lblUsuario);
        formulario.Controls.Add(txtUsuario);
        formulario.Controls.Add(lblClave);
        formulario.Controls.Add(txtClave);
        formulario.Controls.Add(chkMostrar);
        formulario.Controls.Add(btnIngresar);
        formulario.Controls.Add(separador);
        formulario.Controls.Add(btnRegistrarse);

        derecha.Controls.Add(pie);
        derecha.Controls.Add(formulario);
        derecha.Resize += (s, e) => CentrarFormulario();

        Controls.Add(derecha);      // Fill primero
        Controls.Add(izquierda);    // luego el panel Left
        CentrarFormulario();
        ResumeLayout(true);
    }

    private void CentrarFormulario()
    {
        if (derecha == null || formulario == null) return;
        formulario.Location = new Point(Math.Max(0, (derecha.Width - formulario.Width) / 2),
                                        Math.Max(0, (derecha.Height - formulario.Height) / 2 - Estilo.Px(10)));
    }

    // Dibuja la marca: círculos suaves, el nombre, el auto y tres ventajas del sistema
    private void PintarMarca(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        int w = izquierda.Width;
        int h = izquierda.Height;

        using (SolidBrush b1 = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            g.FillEllipse(b1, -Estilo.Px(140), -Estilo.Px(140), Estilo.Px(420), Estilo.Px(420));
        using (SolidBrush b2 = new SolidBrush(Color.FromArgb(22, 245, 158, 11)))
            g.FillEllipse(b2, w - Estilo.Px(240), h - Estilo.Px(280), Estilo.Px(460), Estilo.Px(460));

        using (Font fMarca = new Font("Segoe UI Semibold", 34F, FontStyle.Bold))
        using (Font fLema = new Font("Segoe UI", 12F))
        using (Font fItem = new Font("Segoe UI", 11F))
        using (SolidBrush blanco = new SolidBrush(Color.White))
        using (SolidBrush suave = new SolidBrush(Color.FromArgb(186, 200, 225)))
        using (SolidBrush ambar = new SolidBrush(Estilo.Ambar))
        {
            g.DrawString("RentCar", fMarca, blanco, Estilo.Px(52), Estilo.Px(56));
            g.DrawString("Alquiler de vehículos, bien administrado.", fLema, suave, Estilo.Px(56), Estilo.Px(122));

            // carretera y auto
            float anchoAuto = w - Estilo.Px(120);
            float altoAuto = anchoAuto * 74f / 192f;
            float yAuto = Estilo.Px(210);
            Decor.Auto(g, Estilo.Px(60), yAuto, anchoAuto, Estilo.Ambar);
            using (Pen ruta = new Pen(Color.FromArgb(70, 255, 255, 255), 2f))
            {
                ruta.DashStyle = DashStyle.Dash;
                g.DrawLine(ruta, 0, yAuto + altoAuto + Estilo.Px(16), w, yAuto + altoAuto + Estilo.Px(16));
            }

            string[] ventajas = new string[]
            {
                "Reservas sin cruces de fechas",
                "Cobros y recargos calculados automáticamente",
                "Reportes listos para imprimir en PDF"
            };
            float yItem = yAuto + altoAuto + Estilo.Px(52);
            foreach (string v in ventajas)
            {
                g.FillEllipse(ambar, Estilo.Px(58), yItem + Estilo.Px(7), Estilo.Px(9), Estilo.Px(9));
                g.DrawString(v, fItem, blanco, Estilo.Px(78), yItem);
                yItem += Estilo.Px(32);
            }
        }
    }
}
