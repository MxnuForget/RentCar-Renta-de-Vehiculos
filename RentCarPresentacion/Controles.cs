using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// =====================================================================
//  Controles visuales propios del sistema (dibujo del auto, cabecera,
//  buscador de tablas y botones del menú). No tienen lógica de negocio.
// =====================================================================

// ---------- Dibujos reutilizables ----------
public static class Decor
{
    public static Color Oscurecer(Color c, float f)
    {
        return Color.FromArgb(c.A, (int)(c.R * (1f - f)), (int)(c.G * (1f - f)), (int)(c.B * (1f - f)));
    }

    // Rectángulo con esquinas redondeadas
    public static GraphicsPath Redondeado(Rectangle r, int radio)
    {
        GraphicsPath p = new GraphicsPath();
        int d = radio * 2;
        if (d <= 0 || r.Width < d || r.Height < d)
        {
            p.AddRectangle(r);
            return p;
        }
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    // Dibuja un auto de lado (mirando a la derecha). Ocupa 'ancho' píxeles de ancho
    // y aproximadamente ancho * 0.39 de alto, empezando en (x, y).
    public static void Auto(Graphics g, float x, float y, float ancho, Color carro)
    {
        float k = ancho / 192f;
        GraphicsState estado = g.Save();
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TranslateTransform(x - 8f * k, y - 8f * k);
        g.ScaleTransform(k, k);

        // sombra sobre el piso
        using (SolidBrush sombra = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
            g.FillEllipse(sombra, 14f, 74f, 178f, 10f);

        // carrocería
        using (GraphicsPath cuerpo = new GraphicsPath())
        {
            cuerpo.AddBezier(8f, 52f, 5f, 46f, 8f, 38f, 18f, 36f);
            cuerpo.AddLine(18f, 36f, 48f, 32f);
            cuerpo.AddBezier(48f, 32f, 62f, 17f, 80f, 8f, 102f, 8f);
            cuerpo.AddBezier(102f, 8f, 124f, 8f, 142f, 14f, 154f, 30f);
            cuerpo.AddLine(154f, 30f, 184f, 36f);
            cuerpo.AddBezier(184f, 36f, 194f, 38f, 198f, 45f, 197f, 52f);
            cuerpo.AddLine(197f, 52f, 197f, 58f);
            cuerpo.AddLine(197f, 58f, 184f, 62f);
            cuerpo.AddLine(184f, 62f, 14f, 62f);
            cuerpo.AddLine(14f, 62f, 8f, 58f);
            cuerpo.CloseFigure();
            using (SolidBrush b = new SolidBrush(carro)) g.FillPath(b, cuerpo);
        }

        // franja inferior más oscura
        PointF[] baja = new PointF[]
        {
            new PointF(8f, 52f), new PointF(197f, 50f), new PointF(197f, 58f),
            new PointF(184f, 62f), new PointF(14f, 62f), new PointF(8f, 58f)
        };
        using (SolidBrush b = new SolidBrush(Oscurecer(carro, 0.2f))) g.FillPolygon(b, baja);

        // vidrios
        using (SolidBrush vidrio = new SolidBrush(Color.FromArgb(186, 230, 253)))
        {
            using (GraphicsPath v1 = new GraphicsPath())
            {
                v1.AddBezier(58f, 31f, 68f, 21f, 82f, 14f, 100f, 14f);
                v1.AddLine(100f, 14f, 100f, 31f);
                v1.CloseFigure();
                g.FillPath(vidrio, v1);
            }
            using (GraphicsPath v2 = new GraphicsPath())
            {
                v2.AddLine(106f, 31f, 106f, 14f);
                v2.AddBezier(106f, 14f, 124f, 14f, 138f, 20f, 147f, 31f);
                v2.CloseFigure();
                g.FillPath(vidrio, v2);
            }
        }

        // línea de luz en el costado
        using (Pen linea = new Pen(Color.FromArgb(170, 255, 255, 255), 1.6f))
            g.DrawLine(linea, 20f, 46f, 180f, 47f);

        // ruedas
        float[] centros = new float[] { 48f, 150f };
        foreach (float cx in centros)
        {
            using (SolidBrush hueco = new SolidBrush(Color.FromArgb(10, 14, 25))) g.FillEllipse(hueco, cx - 19f, 43f, 38f, 38f);
            using (SolidBrush llanta = new SolidBrush(Color.FromArgb(30, 41, 59))) g.FillEllipse(llanta, cx - 15f, 47f, 30f, 30f);
            using (SolidBrush aro = new SolidBrush(Color.FromArgb(203, 213, 225))) g.FillEllipse(aro, cx - 9f, 53f, 18f, 18f);
            using (SolidBrush centro = new SolidBrush(Color.FromArgb(71, 85, 105))) g.FillEllipse(centro, cx - 3f, 59f, 6f, 6f);
        }

        // luces
        using (SolidBrush faro = new SolidBrush(Color.FromArgb(253, 224, 71))) g.FillEllipse(faro, 186f, 40f, 8f, 5f);
        using (SolidBrush trasera = new SolidBrush(Color.FromArgb(239, 68, 68))) g.FillEllipse(trasera, 8f, 42f, 5f, 5f);

        g.Restore(estado);
    }
}

// ---------- Cabecera con degradado (y auto) ----------
public class PanelDegradado : Panel
{
    public string Titulo { get; set; }
    public string Subtitulo { get; set; }
    public bool MostrarAuto { get; set; }
    public int AutoAncho { get; set; }
    public Color Color1 { get; set; }
    public Color Color2 { get; set; }
    public LinearGradientMode Direccion { get; set; }

    public PanelDegradado()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        MostrarAuto = true;
        AutoAncho = Estilo.Px(150);
        Color1 = Estilo.Oscuro;
        Color2 = Estilo.Marino;
        Direccion = LinearGradientMode.Horizontal;
        Titulo = "";
        Subtitulo = "";
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Width > 1 && Height > 1)
        {
            Graphics g = e.Graphics;
            using (LinearGradientBrush br = new LinearGradientBrush(ClientRectangle, Color1, Color2, Direccion))
                g.FillRectangle(br, ClientRectangle);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (MostrarAuto)
            {
                float w = AutoAncho;
                float h = w * 74f / 192f;
                Decor.Auto(g, Width - w - Estilo.Px(36), (Height - h) / 2f, w, Estilo.Ambar);
            }

            if (!string.IsNullOrEmpty(Titulo))
            {
                using (Font f1 = new Font("Segoe UI Semibold", 18F, FontStyle.Bold))
                using (SolidBrush b1 = new SolidBrush(Color.White))
                    g.DrawString(Titulo, f1, b1, Estilo.Px(28), Estilo.Px(12));
            }
            if (!string.IsNullOrEmpty(Subtitulo))
            {
                using (Font f2 = new Font("Segoe UI", 10F))
                using (SolidBrush b2 = new SolidBrush(Color.FromArgb(186, 200, 225)))
                    g.DrawString(Subtitulo, f2, b2, Estilo.Px(30), Estilo.Px(48));
            }
        }
        base.OnPaint(e);
    }
}

// ---------- Buscador para cualquier tabla (DataGridView) ----------
// Filtra las filas ya cargadas: no hace consultas a la base de datos.
public class BarraBusqueda : Panel
{
    private DataGridView grilla;
    private TextBox txt = new TextBox();
    private ComboBox cbo = new ComboBox();
    private Label lblCantidad = new Label();
    private List<string> nombres = new List<string>();
    private List<string> firmas = new List<string>();
    private bool ignorar = false;

    public BarraBusqueda(DataGridView g)
    {
        grilla = g;
        Dock = DockStyle.Top;
        Height = Estilo.Px(60);
        BackColor = Estilo.Fondo;

        Label lb = new Label();
        lb.Text = "Buscar:";
        lb.AutoSize = true;
        lb.Font = Estilo.FuenteEtiqueta;
        lb.ForeColor = Estilo.Texto;
        lb.Location = new Point(0, Estilo.Px(21));

        txt.Font = Estilo.FuenteEntrada;
        txt.BorderStyle = BorderStyle.FixedSingle;
        txt.Width = Estilo.Px(300);
        txt.Location = new Point(Estilo.Px(66), Estilo.Px(14));

        Label lb2 = new Label();
        lb2.Text = "en:";
        lb2.AutoSize = true;
        lb2.Font = Estilo.FuenteEtiqueta;
        lb2.ForeColor = Estilo.Texto;
        lb2.Location = new Point(Estilo.Px(66 + 300 + 18), Estilo.Px(21));

        cbo.Font = Estilo.FuenteEntrada;
        cbo.DropDownStyle = ComboBoxStyle.DropDownList;
        cbo.Width = Estilo.Px(220);
        cbo.Location = new Point(Estilo.Px(66 + 300 + 18 + 32), Estilo.Px(14));

        lblCantidad.AutoSize = false;
        lblCantidad.Size = new Size(Estilo.Px(260), Estilo.Px(26));
        lblCantidad.TextAlign = ContentAlignment.MiddleRight;
        lblCantidad.Font = Estilo.FuenteEtiqueta;
        lblCantidad.ForeColor = Estilo.Suave;
        lblCantidad.Top = Estilo.Px(19);

        Controls.Add(lb);
        Controls.Add(txt);
        Controls.Add(lb2);
        Controls.Add(cbo);
        Controls.Add(lblCantidad);

        RellenarColumnas();

        txt.TextChanged += (s, e) => Filtrar();
        txt.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Escape) { txt.Clear(); e.SuppressKeyPress = true; }
        };
        cbo.SelectedIndexChanged += (s, e) => Filtrar();
        cbo.Enter += (s, e) => RellenarColumnas();
        grilla.DataBindingComplete += (s, e) => { RellenarColumnas(); Filtrar(); };
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        lblCantidad.Left = Width - lblCantidad.Width;
    }

    // La lista "en:" contiene las columnas visibles de la tabla
    private void RellenarColumnas()
    {
        List<string> nuevosNombres = new List<string>();
        List<string> nuevasFirmas = new List<string>();
        foreach (DataGridViewColumn c in grilla.Columns)
        {
            if (!c.Visible) continue;
            nuevosNombres.Add(c.Name);
            nuevasFirmas.Add(c.Name + "|" + c.HeaderText);
        }

        bool igual = nuevasFirmas.Count == firmas.Count && cbo.Items.Count > 0;
        if (igual)
        {
            for (int i = 0; i < nuevasFirmas.Count; i++)
            {
                if (nuevasFirmas[i] != firmas[i]) { igual = false; break; }
            }
        }
        if (igual) return;

        string actual = (cbo.SelectedIndex > 0 && cbo.SelectedIndex - 1 < nombres.Count) ? nombres[cbo.SelectedIndex - 1] : null;

        ignorar = true;
        nombres = nuevosNombres;
        firmas = nuevasFirmas;
        cbo.Items.Clear();
        cbo.Items.Add("Todas las columnas");
        foreach (DataGridViewColumn c in grilla.Columns)
        {
            if (c.Visible) cbo.Items.Add(c.HeaderText);
        }
        int pos = actual == null ? -1 : nombres.IndexOf(actual);
        cbo.SelectedIndex = pos + 1;
        ignorar = false;
    }

    private void Filtrar()
    {
        if (ignorar) return;
        try
        {
            string t = txt.Text.Trim().ToLowerInvariant();
            string soloColumna = null;
            if (cbo.SelectedIndex > 0 && cbo.SelectedIndex - 1 < nombres.Count) soloColumna = nombres[cbo.SelectedIndex - 1];

            int total = 0, visibles = 0;
            if (grilla.CurrentCell != null) grilla.CurrentCell = null;   // una fila actual no se puede ocultar
            for (int i = 0; i < grilla.Rows.Count; i++)
            {
                DataGridViewRow fila = grilla.Rows[i];
                if (fila.IsNewRow) continue;
                total++;
                bool ok = t.Length == 0 || Coincide(fila, t, soloColumna);
                if (fila.Visible != ok) fila.Visible = ok;
                if (ok) visibles++;
            }
            lblCantidad.Text = t.Length == 0 ? total + " registros" : visibles + " de " + total + " registros";
        }
        catch (Exception)
        {
            // el buscador nunca debe cerrar el programa
        }
    }

    private bool Coincide(DataGridViewRow fila, string t, string soloColumna)
    {
        foreach (DataGridViewCell c in fila.Cells)
        {
            DataGridViewColumn col = c.OwningColumn;
            if (col == null || !col.Visible) continue;
            if (soloColumna != null && col.Name != soloColumna) continue;
            object v = c.Value;
            if (v == null || v == DBNull.Value) continue;
            string s;
            if (v is DateTime) s = ((DateTime)v).ToString("dd/MM/yyyy");
            else if (v is decimal) s = ((decimal)v).ToString("N2") + " " + ((decimal)v).ToString();
            else s = Convert.ToString(v);
            if (s.ToLowerInvariant().Contains(t)) return true;
        }
        return false;
    }
}

// ---------- Botón del menú lateral ----------
public class BotonNav : Button
{
    public bool Activo { get; set; }
    private bool encima = false;

    public BotonNav()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;
        TabStop = false;
    }

    protected override void OnMouseEnter(EventArgs e) { encima = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { encima = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs pe)
    {
        Graphics g = pe.Graphics;
        Color fondo = Activo ? Estilo.Marino : (encima ? Color.FromArgb(30, 41, 59) : Estilo.Oscuro);
        using (SolidBrush b = new SolidBrush(fondo)) g.FillRectangle(b, ClientRectangle);
        if (Activo)
        {
            using (SolidBrush a = new SolidBrush(Estilo.Ambar)) g.FillRectangle(a, 0, 0, Estilo.Px(4), Height);
        }
        Color texto = Activo ? Color.White : Color.FromArgb(203, 213, 225);
        TextRenderer.DrawText(g, Text, Font, new Rectangle(Estilo.Px(24), 0, Width - Estilo.Px(24), Height), texto,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}

// ---------- Tarjeta de acceso rápido (pantalla de inicio) ----------
public class BotonTarjeta : Button
{
    public string Descripcion { get; set; }
    public Color Acento { get; set; }
    private bool encima = false;

    public BotonTarjeta()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;
        TabStop = false;
        Descripcion = "";
        Acento = Estilo.Azul;
    }

    protected override void OnMouseEnter(EventArgs e) { encima = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { encima = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs pe)
    {
        Graphics g = pe.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Estilo.Fondo);
        Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
        using (GraphicsPath gp = Decor.Redondeado(r, Estilo.Px(10)))
        {
            using (SolidBrush b = new SolidBrush(Color.White)) g.FillPath(b, gp);
            using (Pen p = new Pen(encima ? Acento : Estilo.Borde, encima ? 2f : 1f)) g.DrawPath(p, gp);
        }
        using (SolidBrush barra = new SolidBrush(Acento))
            g.FillRectangle(barra, Estilo.Px(18), Estilo.Px(20), Estilo.Px(5), Estilo.Px(Math.Max(20, 36)));
        TextRenderer.DrawText(g, Text, Estilo.FuenteTarjeta, new Rectangle(Estilo.Px(34), Estilo.Px(14), Width - Estilo.Px(48), Estilo.Px(30)),
            Estilo.Oscuro, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, Descripcion, Estilo.FuenteNota, new Rectangle(Estilo.Px(34), Estilo.Px(46), Width - Estilo.Px(48), Height - Estilo.Px(54)),
            Estilo.Suave, TextFormatFlags.Left | TextFormatFlags.WordBreak);
    }
}
