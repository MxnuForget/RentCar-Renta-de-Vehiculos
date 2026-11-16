using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

// Pantalla de REPORTES: 5 pestañas (TabControl). Solo muestra datos y llama a ReporteNegocio: no tiene SQL.
public class FrmReportes : Form
{
    private ReporteNegocio negocio = new ReporteNegocio();
    private CatalogoNegocio catalogo = new CatalogoNegocio();

    private const string TextoInicial = "Presione Consultar para ver el reporte.";

    // Filtros
    private DateTimePicker ini1, fin1, ini2, fin2, ini3, fin3;
    private ComboBox cboEstado, cboMetodo, cboCategoria, cboSucursal;
    private NumericUpDown numMin;
    private TabControl tabs;

    // Tablas (g1r = resumen por estado del reporte 1) y etiquetas de totales
    private DataGridView g1, g1r, g2, g3, g4, g5;
    private Label res1, res2, res3, res4, res5;

    // Texto de los filtros usados en la última consulta (se imprime en el reporte PDF)
    private string fil1, fil2, fil3, fil4, fil5;

    public FrmReportes()
    {
        Text = "RentCar - Reportes"; 

        Panel pnlTitulo = new Panel { Name = "pnlTitulo", Dock = DockStyle.Top, Height = 56 };
        pnlTitulo.Controls.Add(new Label { Name = "lblTitulo", Text = "Reportes del sistema" });

        tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(14, 6) };
        tabs.TabPages.Add(CrearReporte1());
        tabs.TabPages.Add(CrearReporte2());
        tabs.TabPages.Add(CrearReporte3());
        tabs.TabPages.Add(CrearReporte4());
        tabs.TabPages.Add(CrearReporte5());

        Controls.Add(tabs);        // Fill primero, luego el panel Top
        Controls.Add(pnlTitulo);
        Estilo.Aplicar(this);
        Load += FrmReportes_Load;
    }

    // Lo usa el botón "Ver reporte" de las otras pantallas
    public void MostrarPestana(int indice)
    {
        if (indice >= 0 && indice < tabs.TabPages.Count) tabs.SelectedIndex = indice;
    }

    // Los ComboBox con datos de la base se llenan al abrir la pantalla
    private void FrmReportes_Load(object sender, EventArgs e)
    {
        Ejecutar(() =>
        {
            LlenarCombo(cboMetodo, catalogo.MetodosPago());
            LlenarCombo(cboCategoria, negocio.Categorias());
            LlenarCombo(cboSucursal, catalogo.Sucursales());
        });
    }

    // ======================= REPORTE 1: Alquileres por fecha y estado =======================
    private TabPage CrearReporte1()
    {
        ini1 = Fecha(); fin1 = Fecha(); FechasPorDefecto(ini1, fin1);
        cboEstado = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Margin = new Padding(3, 6, 3, 3) };
        cboEstado.Items.AddRange(new object[] { "Todos", Estados.Activo, Estados.Finalizado, Estados.Cancelado });
        cboEstado.SelectedIndex = 0;

        g1 = Grilla();
        g1r = Grilla(); g1r.Dock = DockStyle.Bottom; g1r.Height = 130;
        res1 = Resumen();

        FlowLayoutPanel filtros = Filtros(
            Etiqueta("Desde:"), ini1, Etiqueta("Hasta:"), fin1, Etiqueta("Estado:"), cboEstado,
            Boton("btnBuscar", "Consultar", (s, e) => Ejecutar(Consultar1)),
            Boton("btnLimpiar", "Limpiar", (s, e) => Ejecutar(Limpiar1)),
            Boton("btnPdf", "Reporte PDF", (s, e) => Ejecutar(Pdf1)));
        return Pestana("1. Alquileres", filtros, g1, res1, g1r);
    }

    private void Consultar1()
    {
        string estado = Convert.ToString(cboEstado.SelectedItem);
        DataTable dt = negocio.Alquileres(ini1.Value, fin1.Value, estado);
        DataTable rs = negocio.ResumenAlquileres(ini1.Value, fin1.Value, estado);
        Mostrar(g1, dt);
        Mostrar(g1r, rs);
        string detalle = "";
        foreach (DataRow f in rs.Rows) detalle += "   |   " + f["estado"] + ": " + f["cantidad"];
        res1.Text = "Total de alquileres: " + dt.Rows.Count + detalle;
        fil1 = "Desde " + ini1.Value.ToString("dd/MM/yyyy") + "   Hasta " + fin1.Value.ToString("dd/MM/yyyy") + "   Estado: " + estado;
        if (dt.Rows.Count == 0) Aviso("No se encontraron alquileres con esos filtros.");
    }

    private void Limpiar1()
    {
        FechasPorDefecto(ini1, fin1);
        cboEstado.SelectedIndex = 0;
        g1.DataSource = null; g1r.DataSource = null;
        res1.Text = TextoInicial;
    }

    // ======================= REPORTE 2: Ingresos por pagos =======================
    private TabPage CrearReporte2()
    {
        ini2 = Fecha(); fin2 = Fecha(); FechasPorDefecto(ini2, fin2);
        cboMetodo = ComboDatos();
        g2 = Grilla();
        res2 = Resumen();

        FlowLayoutPanel filtros = Filtros(
            Etiqueta("Desde:"), ini2, Etiqueta("Hasta:"), fin2, Etiqueta("Método de pago:"), cboMetodo,
            Boton("btnBuscar", "Consultar", (s, e) => Ejecutar(Consultar2)),
            Boton("btnLimpiar", "Limpiar", (s, e) => Ejecutar(Limpiar2)),
            Boton("btnPdf", "Reporte PDF", (s, e) => Ejecutar(Pdf2)));
        return Pestana("2. Ingresos", filtros, g2, res2, null);
    }

    private void Consultar2()
    {
        int met = Convert.ToInt32(cboMetodo.SelectedValue);
        DataTable dt = negocio.Pagos(ini2.Value, fin2.Value, met);
        Mostrar(g2, dt);
        fil2 = "Desde " + ini2.Value.ToString("dd/MM/yyyy") + "   Hasta " + fin2.Value.ToString("dd/MM/yyyy") + "   Método de pago: " + cboMetodo.Text;
        if (dt.Rows.Count == 0)
        {
            res2.Text = "Sin pagos en el período seleccionado.";
            Aviso("No se encontraron pagos con esos filtros.");
            return;
        }
        DataRow r = negocio.ResumenPagos(ini2.Value, fin2.Value, met).Rows[0];
        res2.Text = "Pagos: " + r["num_pagos"] + "   |   Total recaudado: " + Bs(r["total"]) +
                    "   |   Promedio: " + Bs(r["promedio"]) + "   |   Pago máximo: " + Bs(r["maximo"]);
    }

    private void Limpiar2()
    {
        FechasPorDefecto(ini2, fin2);
        if (cboMetodo.Items.Count > 0) cboMetodo.SelectedIndex = 0;
        g2.DataSource = null;
        res2.Text = TextoInicial;
    }

    // ======================= REPORTE 3: Vehículos más alquilados =======================
    private TabPage CrearReporte3()
    {
        ini3 = Fecha(); fin3 = Fecha(); FechasPorDefecto(ini3, fin3);
        cboCategoria = ComboDatos();
        g3 = Grilla();
        res3 = Resumen();

        FlowLayoutPanel filtros = Filtros(
            Etiqueta("Desde:"), ini3, Etiqueta("Hasta:"), fin3, Etiqueta("Categoría:"), cboCategoria,
            Boton("btnBuscar", "Consultar", (s, e) => Ejecutar(Consultar3)),
            Boton("btnLimpiar", "Limpiar", (s, e) => Ejecutar(Limpiar3)),
            Boton("btnPdf", "Reporte PDF", (s, e) => Ejecutar(Pdf3)));
        return Pestana("3. Más alquilados", filtros, g3, res3, null);
    }

    private void Consultar3()
    {
        DataTable dt = negocio.MasAlquilados(ini3.Value, fin3.Value, Convert.ToInt32(cboCategoria.SelectedValue));
        Mostrar(g3, dt);
        fil3 = "Desde " + ini3.Value.ToString("dd/MM/yyyy") + "   Hasta " + fin3.Value.ToString("dd/MM/yyyy") + "   Categoría: " + cboCategoria.Text;
        if (dt.Rows.Count == 0)
        {
            res3.Text = "No hay vehículos para esa categoría.";
            Aviso("No se encontraron vehículos con esos filtros.");
            return;
        }
        DataRow top = dt.Rows[0];
        string lider = Convert.ToInt32(top["alquileres"]) == 0
            ? "ninguno (sin alquileres en el período)"
            : top["placa"] + " (" + top["modelo"] + ") con " + top["alquileres"] + " alquileres";
        res3.Text = "Vehículos: " + dt.Rows.Count + "   |   Alquileres totales: " + dt.Compute("SUM(alquileres)", "") +
                    "   |   Más alquilado: " + lider;
    }

    private void Limpiar3()
    {
        FechasPorDefecto(ini3, fin3);
        if (cboCategoria.Items.Count > 0) cboCategoria.SelectedIndex = 0;
        g3.DataSource = null;
        res3.Text = TextoInicial;
    }

    // ======================= REPORTE 4: Flota por categoría =======================
    private TabPage CrearReporte4()
    {
        cboSucursal = ComboDatos();
        g4 = Grilla();
        res4 = Resumen();

        FlowLayoutPanel filtros = Filtros(
            Etiqueta("Sucursal:"), cboSucursal,
            Boton("btnBuscar", "Consultar", (s, e) => Ejecutar(Consultar4)),
            Boton("btnLimpiar", "Limpiar", (s, e) => Ejecutar(Limpiar4)),
            Boton("btnPdf", "Reporte PDF", (s, e) => Ejecutar(Pdf4)));
        return Pestana("4. Flota", filtros, g4, res4, null);
    }

    private void Consultar4()
    {
        DataTable dt = negocio.FlotaPorCategoria(Convert.ToInt32(cboSucursal.SelectedValue));
        Mostrar(g4, dt);
        fil4 = "Sucursal: " + cboSucursal.Text;
        if (dt.Rows.Count == 0)
        {
            res4.Text = "No hay vehículos en esa sucursal.";
            Aviso("No se encontraron vehículos en esa sucursal.");
            return;
        }
        if (g4.Columns["porcentaje"] != null) g4.Columns["porcentaje"].DefaultCellStyle.Format = "0.0\"%\"";
        DataRow top = dt.Rows[0];
        res4.Text = "Total de vehículos: " + dt.Compute("SUM(vehiculos)", "") + "   |   Categorías: " + dt.Rows.Count +
                    "   |   Mayor participación: " + top["categoria"] + " (" + top["porcentaje"] + "%)";
    }

    private void Limpiar4()
    {
        if (cboSucursal.Items.Count > 0) cboSucursal.SelectedIndex = 0;
        g4.DataSource = null;
        res4.Text = TextoInicial;
    }

    // ======================= REPORTE 5: Clientes frecuentes =======================
    private TabPage CrearReporte5()
    {
        numMin = new NumericUpDown { Minimum = 0, Maximum = 100, Value = 1, Width = 70, Margin = new Padding(3, 6, 3, 3) };
        g5 = Grilla();
        res5 = Resumen();

        FlowLayoutPanel filtros = Filtros(
            Etiqueta("Mínimo de alquileres:"), numMin,
            Boton("btnBuscar", "Consultar", (s, e) => Ejecutar(Consultar5)),
            Boton("btnLimpiar", "Limpiar", (s, e) => Ejecutar(Limpiar5)),
            Boton("btnPdf", "Reporte PDF", (s, e) => Ejecutar(Pdf5)));
        return Pestana("5. Clientes frecuentes", filtros, g5, res5, null);
    }

    private void Consultar5()
    {
        DataTable dt = negocio.ClientesFrecuentes((int)numMin.Value);
        Mostrar(g5, dt);
        fil5 = "Mínimo de alquileres: " + numMin.Value;
        if (g5.Columns["ci"] != null) g5.Columns["ci"].HeaderText = "CI";
        if (dt.Rows.Count == 0)
        {
            res5.Text = "Ningún cliente cumple el mínimo indicado.";
            Aviso("No se encontraron clientes con esa cantidad de alquileres.");
            return;
        }
        res5.Text = "Clientes: " + dt.Rows.Count + "   |   Total pagado por ellos: " + Bs(dt.Compute("SUM(total_pagado)", ""));
    }

    private void Limpiar5()
    {
        numMin.Value = 1;
        g5.DataSource = null;
        res5.Text = TextoInicial;
    }

    // ======================= Reporte en PDF (RDLC) =======================
    private void Pdf1() { AbrirPdf("Reporte1Alquileres.rdlc", "Alquileres por fecha y estado", g1, fil1, res1.Text); }
    private void Pdf2() { AbrirPdf("Reporte2Ingresos.rdlc", "Ingresos por pagos", g2, fil2, res2.Text); }
    private void Pdf3() { AbrirPdf("Reporte3MasAlquilados.rdlc", "Vehículos más alquilados", g3, fil3, res3.Text); }
    private void Pdf4() { AbrirPdf("Reporte4Flota.rdlc", "Flota por categoría", g4, fil4, res4.Text); }
    private void Pdf5() { AbrirPdf("Reporte5Clientes.rdlc", "Clientes frecuentes", g5, fil5, res5.Text); }

    // Abre el visor RDLC con los mismos datos que se ven en la tabla (hay que haber presionado Consultar)
    private void AbrirPdf(string rdlc, string titulo, DataGridView g, string filtros, string resumen)
    {
        DataTable dt = g.DataSource as DataTable;
        if (dt == null)
        {
            Aviso("Primero presione Consultar para generar el reporte.");
            return;
        }
        // si se usó el buscador, el PDF incluye solo las filas que se ven en la tabla
        bool hayOcultas = false;
        for (int i = 0; i < g.Rows.Count; i++) { if (!g.Rows[i].Visible) { hayOcultas = true; break; } }
        if (hayOcultas)
        {
            DataTable copia = dt.Clone();
            for (int i = 0; i < g.Rows.Count; i++)
            {
                DataRowView v = g.Rows[i].DataBoundItem as DataRowView;
                if (g.Rows[i].Visible && v != null) copia.ImportRow(v.Row);
            }
            dt = copia;
            filtros = filtros + "   |   Con búsqueda aplicada";
        }
        if (dt.Rows.Count == 0)
        {
            Aviso("No hay datos para mostrar en el reporte.");
            return;
        }
        using (FrmVisorReporte visor = new FrmVisorReporte(rdlc, titulo, dt, filtros, resumen))
            visor.ShowDialog(this);
    }

    // ======================= Ayudantes de la pantalla (sin lógica de negocio) =======================
    private void Ejecutar(Action accion)
    {
        try { accion(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private static void Aviso(string texto)
    {
        MessageBox.Show(texto, "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static string Bs(object valor)
    {
        return "Bs " + Convert.ToDecimal(valor).ToString("N2");
    }

    private static void Mostrar(DataGridView g, DataTable dt)
    {
        g.DataSource = dt;
        Estilo.Columnas(g);
    }

    private static void FechasPorDefecto(DateTimePicker a, DateTimePicker b)
    {
        a.Value = DateTime.Today.AddMonths(-6);
        b.Value = DateTime.Today.AddMonths(1);
    }

    // Llena un ComboBox con una fila inicial "Todos" (Id 0) y luego los datos (columnas Id y Nombre)
    private static void LlenarCombo(ComboBox cb, DataTable origen)
    {
        DataTable t = new DataTable();
        t.Columns.Add("Id", typeof(int));
        t.Columns.Add("Nombre", typeof(string));
        t.Rows.Add(0, "Todos");
        foreach (DataRow r in origen.Rows)
            t.Rows.Add(Convert.ToInt32(r["Id"]), Convert.ToString(r["Nombre"]));
        cb.DisplayMember = "Nombre";
        cb.ValueMember = "Id";
        cb.DataSource = t;
        cb.SelectedIndex = 0;
    }

    private static ComboBox ComboDatos()
    {
        return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 170, Margin = new Padding(3, 6, 3, 3) };
    }

    private static DateTimePicker Fecha()
    {
        return new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 115, Margin = new Padding(3, 6, 3, 3) };
    }

    private static Label Etiqueta(string texto)
    {
        return new Label { Text = texto, AutoSize = true, Margin = new Padding(10, 10, 3, 3) };
    }

    private static Button Boton(string nombre, string texto, EventHandler clic)
    {
        Button b = new Button { Name = nombre, Text = texto, Width = 105, Height = 32, Margin = new Padding(8, 3, 3, 3) };
        b.Click += clic;
        return b;
    }

    private static DataGridView Grilla()
    {
        return new DataGridView { Dock = DockStyle.Fill };   // el estilo lo pone Estilo.Aplicar
    }

    private static Label Resumen()
    {
        return new Label
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI Semibold", 11F),
            Padding = new Padding(8, 0, 0, 0),
            Text = TextoInicial
        };
    }

    private static FlowLayoutPanel Filtros(params Control[] controles)
    {
        FlowLayoutPanel f = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true,
            MinimumSize = new Size(0, 54),
            Padding = new Padding(6, 6, 6, 4)
        };
        f.Controls.AddRange(controles);
        return f;
    }

    // Orden de agregado: Fill primero, luego los Bottom y al final el Top
    private static TabPage Pestana(string titulo, Control filtros, Control grilla, Control resumen, Control extraAbajo)
    {
        TabPage t = new TabPage(titulo) { UseVisualStyleBackColor = false, BackColor = Estilo.Fondo };
        t.Controls.Add(grilla);
        if (extraAbajo != null) t.Controls.Add(extraAbajo);
        t.Controls.Add(resumen);
        DataGridView dg = grilla as DataGridView;
        if (dg != null) t.Controls.Add(new BarraBusqueda(dg));   // buscador con elección de columna
        t.Controls.Add(filtros);
        return t;
    }
}