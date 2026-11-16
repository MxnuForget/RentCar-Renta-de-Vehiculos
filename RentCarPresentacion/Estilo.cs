using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// =====================================================================
//  Diseño uniforme de RentCar.
//  Estilo.Aplicar(formulario) se llama una vez en cada formulario, justo
//  después de InitializeComponent(). Reordena los controles (etiquetas,
//  cajas, botones y tabla) en un diseño limpio que no se pisa, agrega la
//  cabecera, el buscador de la tabla y unifica el tamaño de las ventanas.
//  No toca la lógica: los nombres de los controles siguen siendo los mismos.
// =====================================================================
public static class Estilo
{
    // ---------- Paleta ----------
    public static readonly Color Fondo = Color.FromArgb(241, 245, 249);
    public static readonly Color Oscuro = Color.FromArgb(15, 23, 42);
    public static readonly Color Marino = Color.FromArgb(30, 58, 138);
    public static readonly Color Texto = Color.FromArgb(51, 65, 85);
    public static readonly Color Suave = Color.FromArgb(100, 116, 139);
    public static readonly Color Borde = Color.FromArgb(226, 232, 240);
    public static readonly Color Azul = Color.FromArgb(37, 99, 235);
    public static readonly Color Verde = Color.FromArgb(22, 163, 74);
    public static readonly Color Rojo = Color.FromArgb(220, 38, 38);
    public static readonly Color Gris = Color.FromArgb(100, 116, 139);
    public static readonly Color Turquesa = Color.FromArgb(8, 145, 178);
    public static readonly Color Ambar = Color.FromArgb(245, 158, 11);

    // ---------- Fuentes ----------
    public static readonly Font FuenteBase = new Font("Segoe UI", 10F);
    public static readonly Font FuenteEntrada = new Font("Segoe UI", 12F);
    public static readonly Font FuenteCompacta = new Font("Segoe UI", 11F);
    public static readonly Font FuenteEtiqueta = new Font("Segoe UI Semibold", 9.5F);
    public static readonly Font FuenteBoton = new Font("Segoe UI Semibold", 10.5F);
    public static readonly Font FuenteTarjeta = new Font("Segoe UI Semibold", 12.5F);
    public static readonly Font FuenteNota = new Font("Segoe UI", 9.5F);
    public static readonly Font FuenteCelda = new Font("Segoe UI", 10.5F);

    // El menú principal asigna esta acción: abre la pantalla de reportes en la pestaña indicada
    public static Action<int> IrAReporte;

    // ---------- Escala (pantallas con 125%, 150%... de zoom) ----------
    private static float escala = 0f;

    public static int Px(int valor)
    {
        if (escala <= 0f)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) escala = g.DpiX / 96f;
            if (escala < 1f) escala = 1f;
        }
        return (int)Math.Round(valor * escala);
    }

    // ---------- Tamaño único de las ventanas ----------
    public static void TamanoEstandar(Form f, int ancho, int alto, bool dialogo)
    {
        Rectangle area = Screen.PrimaryScreen.WorkingArea;
        int w = Math.Min(Px(ancho), area.Width - Px(30));
        int h = Math.Min(Px(alto), area.Height - Px(90));
        f.ClientSize = new Size(w, h);
        f.StartPosition = FormStartPosition.CenterScreen;
        f.FormBorderStyle = dialogo ? FormBorderStyle.FixedDialog : FormBorderStyle.FixedSingle;
        f.MaximizeBox = false;
        f.MinimizeBox = !dialogo;
    }

    // =================================================================
    //  APLICAR: punto de entrada para todos los formularios
    // =================================================================
    public static void Aplicar(Form f)
    {
        f.SuspendLayout();
        f.BackColor = Fondo;
        f.Font = FuenteBase;

        Control[] hijos = new Control[f.Controls.Count];
        f.Controls.CopyTo(hijos, 0);

        bool tieneBuscador = f.Controls.Find("txtBuscar", true).Length > 0;

        Panel viejo = null;
        string titulo = f.Text;
        List<Label> etiquetas = new List<Label>();
        List<Control> entradas = new List<Control>();
        List<Button> botones = new List<Button>();
        List<DataGridView> grillas = new List<DataGridView>();

        foreach (Control c in hijos)
        {
            if (c.Name == "pnlTitulo" && c is Panel)
            {
                viejo = (Panel)c;
                foreach (Control h in viejo.Controls)
                {
                    if (h is Label && h.Name == "lblTitulo" && h.Text.Length > 0) titulo = h.Text;
                }
            }
            else if (c is Label) etiquetas.Add((Label)c);
            else if (c is Button) botones.Add((Button)c);
            else if (c is DataGridView) grillas.Add((DataGridView)c);
            else if (c is TextBox || c is ComboBox || c is DateTimePicker || c is NumericUpDown) entradas.Add(c);
        }

        string subtitulo = "RentCar  ·  Sistema de gestión de alquiler de vehículos";
        bool armar = entradas.Count > 0 || botones.Count > 0 || grillas.Count > 0;

        if (!armar)
        {
            // Formulario ya armado en código (por ejemplo Reportes): solo cabecera y estilo
            if (viejo != null) { f.Controls.Remove(viejo); viejo.Dispose(); }
            f.Controls.Add(Cabecera(titulo, subtitulo));
            foreach (Control c in hijos)
            {
                if (c != viejo) EstilizarArbol(c);
            }
            TamanoEstandar(f, 1180, 700, false);
            f.ResumeLayout(true);
            return;
        }

        // ---------- 1) emparejar cada etiqueta con su caja ----------
        etiquetas.Sort(PorPosicion);
        List<Control> usadas = new List<Control>();
        List<KeyValuePair<Label, Control>> campos = new List<KeyValuePair<Label, Control>>();
        List<Label> notas = new List<Label>();

        foreach (Label lb in etiquetas)
        {
            Control mejor = null;
            foreach (Control en in entradas)
            {
                if (usadas.Contains(en)) continue;
                if (Math.Abs(en.Top - lb.Top) > 14) continue;
                if (en.Left <= lb.Left) continue;
                if (mejor == null || en.Left < mejor.Left) mejor = en;
            }
            if (mejor == null) notas.Add(lb);
            else
            {
                usadas.Add(mejor);
                campos.Add(new KeyValuePair<Label, Control>(lb, mejor));
            }
        }

        // orden de lectura: por columnas (izquierda a derecha) y de arriba hacia abajo
        campos.Sort((a, b) =>
        {
            int ca = a.Key.Left / 150;
            int cb = b.Key.Left / 150;
            if (ca != cb) return ca.CompareTo(cb);
            return a.Key.Top.CompareTo(b.Key.Top);
        });

        // ---------- 2) tarjeta con los campos y los botones ----------
        bool hayGrilla = grillas.Count > 0;
        int anchoCampo = hayGrilla ? 250 : 270;
        FlowLayoutPanel tarjeta = Tarjeta();
        int orden = 0;
        Control ultimo = null;

        foreach (KeyValuePair<Label, Control> par in campos)
        {
            Panel grupo = Campo(par.Key, par.Value, anchoCampo);
            grupo.TabIndex = orden++;
            tarjeta.Controls.Add(grupo);
            ultimo = grupo;
        }
        foreach (Control en in entradas)
        {
            if (usadas.Contains(en)) continue;
            Panel grupo = Campo(null, en, anchoCampo);
            grupo.TabIndex = orden++;
            tarjeta.Controls.Add(grupo);
            ultimo = grupo;
        }
        if (ultimo != null) tarjeta.SetFlowBreak(ultimo, true);

        botones.Sort(PorPosicion);
        foreach (Button b in botones)
        {
            Boton(b, 42);
            b.Margin = new Padding(0, Px(10), Px(12), Px(4));
            b.TabIndex = orden++;
            tarjeta.Controls.Add(b);
        }

        int idReporte = ReporteDe(f);
        if (idReporte >= 0 && IrAReporte != null)
        {
            Button ver = new Button();
            ver.Name = "btnVerReporte";
            ver.Text = "Ver reporte";
            int destino = idReporte;
            ver.Click += (s, e) => { if (IrAReporte != null) IrAReporte(destino); };
            Boton(ver, 42);
            ver.Margin = new Padding(Px(18), Px(10), Px(12), Px(4));
            tarjeta.Controls.Add(ver);
        }

        foreach (Label nota in notas)
        {
            nota.AutoSize = true;
            nota.Font = nota.Name == "lblResumen" ? FuenteEtiqueta : FuenteNota;
            nota.ForeColor = nota.Name == "lblResumen" ? Texto : Suave;
            nota.BackColor = Color.White;
            nota.Margin = new Padding(Px(8), Px(22), 0, 0);
            tarjeta.Controls.Add(nota);
        }

        // ---------- 3) cuerpo: tarjeta + buscador + tabla ----------
        Panel cuerpo = new Panel();
        cuerpo.Dock = DockStyle.Fill;
        cuerpo.BackColor = Fondo;
        cuerpo.Padding = new Padding(Px(24), Px(20), Px(24), Px(16));

        BarraBusqueda barra = null;
        if (hayGrilla)
        {
            DataGridView g = grillas[0];
            Grilla(g);
            g.Dock = DockStyle.Fill;

            Panel marco = new Panel();
            marco.Dock = DockStyle.Fill;
            marco.BackColor = Borde;
            marco.Padding = new Padding(1);
            marco.Controls.Add(g);

            cuerpo.Controls.Add(marco);                       // Fill primero
            if (!tieneBuscador)
            {
                barra = new BarraBusqueda(g);
                cuerpo.Controls.Add(barra);                   // Top
            }
            else
            {
                Panel separador = new Panel();
                separador.Dock = DockStyle.Top;
                separador.Height = Px(14);
                cuerpo.Controls.Add(separador);
            }

            cuerpo.AutoScroll = true;
            Action ajustar = delegate ()
            {
                int minimo = tarjeta.Height + (barra != null ? barra.Height : Px(14)) + Px(240) + cuerpo.Padding.Vertical;
                if (cuerpo.AutoScrollMinSize.Height != minimo) cuerpo.AutoScrollMinSize = new Size(0, minimo);
            };
            tarjeta.SizeChanged += (s, e) => ajustar();
        }
        cuerpo.Controls.Add(tarjeta);                         // Top (queda arriba de todo)

        // ---------- 4) formulario ----------
        if (viejo != null) { f.Controls.Remove(viejo); viejo.Dispose(); }
        f.Controls.Add(cuerpo);
        f.Controls.Add(Cabecera(titulo, subtitulo));

        if (hayGrilla) TamanoEstandar(f, 1180, 700, false);
        else TamanoEstandar(f, 1000, 640, true);

        // ---------- 5) reglas de texto (mayúsculas en nombres, CI) ----------
        foreach (Control c in entradas)
        {
            TextBox tb = c as TextBox;
            if (tb != null) ConfigurarTexto(tb);
        }

        f.ResumeLayout(true);
    }

    // =================================================================
    //  Piezas reutilizables (también las usan el login, el menú y el portal)
    // =================================================================
    public static PanelDegradado Cabecera(string titulo, string subtitulo)
    {
        PanelDegradado c = new PanelDegradado();
        c.Dock = DockStyle.Top;
        c.Height = Px(78);
        c.Titulo = titulo;
        c.Subtitulo = subtitulo;
        return c;
    }

    public static FlowLayoutPanel Tarjeta()
    {
        FlowLayoutPanel t = new FlowLayoutPanel();
        t.Dock = DockStyle.Top;
        t.AutoSize = true;
        t.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        t.WrapContents = true;
        t.FlowDirection = FlowDirection.LeftToRight;
        t.BackColor = Color.White;
        t.Padding = new Padding(Px(22), Px(18), Px(10), Px(12));
        t.Paint += (s, e) =>
        {
            using (Pen p = new Pen(Borde)) e.Graphics.DrawRectangle(p, 0, 0, t.Width - 1, t.Height - 1);
        };
        t.SizeChanged += (s, e) => t.Invalidate();
        return t;
    }

    // Etiqueta arriba y caja grande abajo. Si lb es null se crea una etiqueta vacía.
    public static Panel Campo(Label lb, Control en, int anchoPx)
    {
        int w = Px(anchoPx);
        Panel g = new Panel();
        g.BackColor = Color.White;
        g.Margin = new Padding(0, 0, Px(18), Px(6));

        Label etiqueta = lb != null ? lb : new Label();
        etiqueta.AutoSize = false;
        etiqueta.Font = FuenteEtiqueta;
        etiqueta.ForeColor = Suave;
        etiqueta.BackColor = Color.White;
        etiqueta.Location = new Point(0, 0);
        etiqueta.Size = new Size(w, Px(22));
        etiqueta.TextAlign = ContentAlignment.BottomLeft;

        Entrada(en);
        en.Width = w;
        en.Location = new Point(0, Px(24));
        int h;
        TextBox tb = en as TextBox;
        if (tb != null && tb.Multiline)
        {
            tb.Height = Px(88);
            h = tb.Height;
        }
        else
        {
            h = Math.Max(en.Height, en.PreferredSize.Height);
        }
        g.Size = new Size(w, Px(24) + h + Px(10));
        g.Controls.Add(etiqueta);
        g.Controls.Add(en);
        return g;
    }

    // Caja de texto / lista / fecha / número con letra grande
    public static void Entrada(Control c)
    {
        c.Font = FuenteEntrada;
        TextBox tb = c as TextBox;
        if (tb != null) tb.BorderStyle = BorderStyle.FixedSingle;
    }

    private static void EntradaCompacta(Control c)
    {
        c.Font = FuenteCompacta;
        if (c is DateTimePicker && c.Width < Px(140)) c.Width = Px(140);
        if (c is ComboBox && c.Width < Px(170)) c.Width = Px(170);
        if (c is NumericUpDown && c.Width < Px(90)) c.Width = Px(90);
        TextBox tb = c as TextBox;
        if (tb != null) tb.BorderStyle = BorderStyle.FixedSingle;
    }

    private static int PorPosicion(Control a, Control b)
    {
        int d = a.Top.CompareTo(b.Top);
        return d != 0 ? d : a.Left.CompareTo(b.Left);
    }

    // A qué pestaña del reporte lleva el botón "Ver reporte" de cada pantalla (-1 = ninguna)
    private static int ReporteDe(Form f)
    {
        switch (f.GetType().Name)
        {
            case "FrmAlquiler": return 0;
            case "FrmAdminAlquileres": return 0;
            case "FrmPago": return 1;
            case "FrmMetodoPago": return 1;
            case "FrmCliente": return 4;
            default: return -1;
        }
    }

    // Reglas de escritura: nombres con mayúscula inicial y CI solo con caracteres válidos
    private static void ConfigurarTexto(TextBox tb)
    {
        if (tb.Name == "txtNombres" || tb.Name == "txtApPaterno" || tb.Name == "txtApMaterno")
        {
            tb.Leave += (s, e) =>
            {
                string nuevo = Validaciones.Capitalizar(tb.Text);
                if (nuevo != tb.Text) tb.Text = nuevo;
            };
        }
        else if (tb.Name == "txtCi")
        {
            tb.MaxLength = 12;
            tb.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-') e.Handled = true;
            };
        }
    }

    private static void EstilizarArbol(Control c)
    {
        Button b = c as Button;
        if (b != null) { Boton(b, 38); return; }
        DataGridView g = c as DataGridView;
        if (g != null) { Grilla(g); return; }
        if (c is TextBox || c is ComboBox || c is DateTimePicker || c is NumericUpDown) { EntradaCompacta(c); return; }
        TabControl tc = c as TabControl;
        if (tc != null)
        {
            tc.Font = FuenteBoton;
            tc.Padding = new Point(Px(18), Px(8));
        }
        if (c is Label && c.ForeColor == SystemColors.ControlText) c.ForeColor = Texto;
        foreach (Control h in c.Controls) EstilizarArbol(h);
    }

    // =================================================================
    //  Botones redondeados
    // =================================================================
    private static Color ColorBoton(string nombre)
    {
        switch (nombre)
        {
            case "btnRegistrar": case "btnCrear": case "btnReservar": return Verde;
            case "btnModificar": case "btnFinalizar": case "btnActivar": case "btnIngresar": return Azul;
            case "btnEliminar": case "btnCancelarReserva": case "btnCancelarAlquiler": return Rojo;
            case "btnLimpiar": case "btnCancelar": case "btnCerrarSesion": return Gris;
            case "btnListar": case "btnBuscar": case "btnFiltrar": case "btnRegistrarse": case "btnVerReporte": return Turquesa;
            case "btnPdf": return Color.FromArgb(79, 70, 229);
            default: return Azul;
        }
    }

    public static void Boton(Button b)
    {
        Boton(b, 42);
    }

    public static void Boton(Button b, int alto)
    {
        Color col = ColorBoton(b.Name);
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.UseVisualStyleBackColor = false;
        b.Font = FuenteBoton;
        b.ForeColor = Color.White;
        b.Cursor = Cursors.Hand;
        b.AutoSize = false;
        int anchoTexto = TextRenderer.MeasureText(b.Text, FuenteBoton).Width + Px(40);
        b.Size = new Size(Math.Max(Math.Max(b.Width, anchoTexto), Px(120)), Px(alto));

        bool encima = false;
        b.MouseEnter += (s, e) => { encima = true; b.Invalidate(); };
        b.MouseLeave += (s, e) => { encima = false; b.Invalidate(); };
        b.Paint += (s, e) => PintarBoton(b, e.Graphics, col, encima);
    }

    private static void PintarBoton(Button b, Graphics g, Color col, bool encima)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Color fondoPadre = (b.Parent != null) ? b.Parent.BackColor : Fondo;
        if (fondoPadre.A < 255) fondoPadre = Fondo;
        g.Clear(fondoPadre);

        Color relleno = !b.Enabled ? Color.FromArgb(203, 213, 225) : (encima ? Decor.Oscurecer(col, 0.14f) : col);
        Rectangle r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
        using (GraphicsPath gp = Decor.Redondeado(r, Px(8)))
        using (SolidBrush br = new SolidBrush(relleno))
            g.FillPath(br, gp);

        Color letra = b.Enabled ? Color.White : Color.FromArgb(100, 116, 139);
        TextRenderer.DrawText(g, b.Text, b.Font, new Rectangle(0, 0, b.Width, b.Height), letra,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    // =================================================================
    //  Tablas
    // =================================================================
    public static void Grilla(DataGridView g)
    {
        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = Color.White;
        g.EnableHeadersVisualStyles = false;
        g.RowHeadersVisible = false;
        g.AllowUserToResizeRows = false;
        g.AllowUserToAddRows = false;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.GridColor = Color.FromArgb(226, 232, 240);
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = Px(42);
        g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.Font = FuenteBoton;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 41, 59);
        g.ColumnHeadersDefaultCellStyle.Padding = new Padding(Px(8), 0, 0, 0);
        g.DefaultCellStyle.Font = FuenteCelda;
        g.DefaultCellStyle.ForeColor = Texto;
        g.DefaultCellStyle.Padding = new Padding(Px(8), 0, 0, 0);
        g.DefaultCellStyle.SelectionBackColor = Azul;
        g.DefaultCellStyle.SelectionForeColor = Color.White;
        g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        g.RowTemplate.Height = Px(38);
        g.ReadOnly = true;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    // Llamar después de asignar DataSource: oculta columnas técnicas y arregla encabezados/formatos
    public static void Columnas(DataGridView g, params string[] ocultas)
    {
        foreach (DataGridViewColumn col in g.Columns)
        {
            if (Array.IndexOf(ocultas, col.Name) >= 0) { col.Visible = false; continue; }
            col.HeaderText = Bonito(col.Name);
            col.MinimumWidth = Px(70);
            if (col.Name == "monto" || col.Name.StartsWith("costo") || col.Name.Contains("total") || col.Name.Contains("tarifa") || col.Name == "pagado" || col.Name == "saldo")
            {
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (col.ValueType == typeof(DateTime)) col.DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        g.ClearSelection();
    }

    public static string Bonito(string nombre)
    {
        if (string.IsNullOrEmpty(nombre)) return "";
        string t = nombre.Replace('_', ' ');
        return char.ToUpper(t[0]) + t.Substring(1);
    }
}
