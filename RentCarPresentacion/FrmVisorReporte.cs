using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

// Visor de reportes RDLC: carga un archivo .rdlc de la carpeta Reportes, le pasa los datos ya consultados
// (los mismos que se ven en la tabla) y permite verlo, imprimirlo y guardarlo en PDF.
// No tiene SQL: los datos llegan desde FrmReportes (que los pidió a la capa de negocio).
public class FrmVisorReporte : Form
{
    private ReportViewer visor = new ReportViewer();
    private string nombreArchivo;

    public FrmVisorReporte(string archivoRdlc, string titulo, DataTable datos, string filtros, string resumen)
    {
        Text = "RentCar - " + titulo;
        Estilo.TamanoEstandar(this, 1180, 700, false);
        nombreArchivo = archivoRdlc.Replace(".rdlc", "") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

        // Barra superior con el botón para guardar el PDF
        Panel barra = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Estilo.Fondo };
        Button btnPdf = new Button { Name = "btnPdf", Text = "Guardar como PDF", Width = 170, Height = 32, Left = 12, Top = 8 };
        btnPdf.Click += (s, e) => GuardarPdf();
        Estilo.Boton(btnPdf, 36);
        btnPdf.Location = new Point(12, 6);
        barra.Controls.Add(btnPdf);

        visor.Dock = DockStyle.Fill;
        visor.ShowParameterPrompts = false;   // los parámetros los enviamos nosotros

        // 1) Cargar el diseño RDLC
        string ruta = Path.Combine(AppContext.BaseDirectory, "Reportes", archivoRdlc);
        if (!File.Exists(ruta))
            throw new FileNotFoundException("No se encontró el archivo del reporte: " + ruta +
                "\nRevise que el .rdlc tenga 'Copiar en el directorio de resultados' = 'Copiar si es posterior'.");
        using (FileStream fs = File.OpenRead(ruta))
            visor.LocalReport.LoadReportDefinition(fs);

        // 2) Datos y parámetros (título, filtros usados, resumen y fecha de emisión)
        visor.LocalReport.DataSources.Clear();
        visor.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ATexto(datos)));
        visor.LocalReport.SetParameters(new ReportParameter[]
        {
            new ReportParameter("Titulo", titulo),
            new ReportParameter("Filtros", string.IsNullOrWhiteSpace(filtros) ? "Ninguno" : filtros),
            new ReportParameter("Resumen", string.IsNullOrWhiteSpace(resumen) ? "-" : resumen),
            new ReportParameter("Generado", "Emitido el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " por " + Sesion.Usuario)
        });

        visor.SetDisplayMode(DisplayMode.PrintLayout);
        Controls.Add(visor);   // Fill primero
        Controls.Add(barra);   // luego el panel Top
        Load += (s, e) => visor.RefreshReport();
    }

    private void GuardarPdf()
    {
        try
        {
            byte[] pdf = visor.LocalReport.Render("PDF");
            using (SaveFileDialog dlg = new SaveFileDialog { Filter = "Archivo PDF (*.pdf)|*.pdf", FileName = nombreArchivo })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                File.WriteAllBytes(dlg.FileName, pdf);
                MessageBox.Show("El PDF se guardó correctamente.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo guardar el PDF: " + ex.Message, "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // Convierte todas las columnas a texto ya formateado (fechas dd/MM/yyyy, montos con 2 decimales),
    // así el diseño RDLC solo tiene que mostrar los campos.
    private static DataTable ATexto(DataTable origen)
    {
        DataTable t = new DataTable("DataSet1");
        foreach (DataColumn c in origen.Columns) t.Columns.Add(c.ColumnName, typeof(string));
        foreach (DataRow fila in origen.Rows)
        {
            DataRow nueva = t.NewRow();
            foreach (DataColumn c in origen.Columns)
                nueva[c.ColumnName] = Formato(fila[c], c.ColumnName);
            t.Rows.Add(nueva);
        }
        return t;
    }

    private static string Formato(object v, string columna)
    {
        if (v == null || v == DBNull.Value) return "";
        if (v is DateTime) return ((DateTime)v).ToString("dd/MM/yyyy");
        if (columna == "porcentaje") return Convert.ToDecimal(v).ToString("0.0") + " %";
        if (v is decimal || v is double || v is float) return Convert.ToDecimal(v).ToString("N2");
        return Convert.ToString(v);
    }
}
