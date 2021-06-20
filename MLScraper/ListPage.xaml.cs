using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary.DAO;

namespace MLScraper
{
    /// <summary>
    /// Lógica de interacción para ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        private ArrayList articulos = new ArrayList();
        public static int code;
        public ListPage()
        {
            InitializeComponent();
            get_articulos();
        }

        private void get_articulos()
        {
            Console.WriteLine("Nueva ListPage");
            ArticuloDAO artDAO = new ArticuloDAO();
            articulos = artDAO.GetArticulos();
            DGArt.ItemsSource = articulos;
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).CommandParameter.ToString());
            ArticuloDAO art = new ArticuloDAO();
            art.deleteArticulo(id);
            MessageBox.Show("Eliminado");
            get_articulos();
        }

        private void btnDetalle_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.StaticMainFrame.Content = new PageDetail(((Button)sender).CommandParameter.ToString()); 
        }

        private void btnComprar(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((Button)sender).CommandParameter.ToString());
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("List Delete");
        }
    }
}
