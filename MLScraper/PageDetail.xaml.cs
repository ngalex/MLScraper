using ClassLibrary.DAO;
using ClassLibrary.Model;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MLScraper
{
    /// <summary>
    /// Lógica de interacción para PageDetail.xaml
    /// </summary>
    public partial class PageDetail : Page
    {
        private ArrayList hArticulos;
        private Articulo articulo;
        
        public PageDetail(string code)
        {
            InitializeComponent();
            ArticuloDAO artDao = new ArticuloDAO();
            articulo = artDao.GetArticulo(code);
            lblName.Text = articulo.Name;
            imgArt.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(articulo.Image);
            GetHistorial_Articulos();
            ucChart.historial = hArticulos;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.StaticMainFrame.GoBack();
        }
        private void GetHistorial_Articulos()
        {
            HistorialArticuloDAO ha = new HistorialArticuloDAO();
            hArticulos = ha.GetHistorialArticulos(int.Parse(articulo.Code));
            hArticulos.Reverse();
            haDG.ItemsSource = hArticulos;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Detail Delete");
        }
    }
}
