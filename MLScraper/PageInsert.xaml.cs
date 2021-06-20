using ClassLibrary.DAO;
using ClassLibrary.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClassLibrary;
using System;

namespace MLScraper
{
    /// <summary>
    /// Lógica de interacción para PageInsert.xaml
    /// </summary>
    public partial class PageInsert : Page
    {
        private bool found;
        private Articulo art;
        public PageInsert()
        {
            InitializeComponent();
            found = false;
            btnBuscar.IsEnabled = false;
            btnAgregar.IsEnabled = false;
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (txtUrl.Text.Length != 0 && Uri.IsWellFormedUriString(txtUrl.Text, UriKind.Absolute))
            {
                art = new Articulo(txtUrl.Text);
                if(art.Status == ArticuloStatus.NO_ENCONTRADO.ToString())
                {
                    MessageBox.Show("NO ENCONTRADO");
                } else
                {
                    txtName.Text = art.Name;
                    txtPrice.Text = art.Price.ToString();
                    imgArt.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(art.Image);
                    found = true;
                    btnAgregar.IsEnabled = true;
                }
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (found)
            {
                ArticuloDAO artDao = new ArticuloDAO();
                //HistorialArticuloDAO haDao = new HistorialArticuloDAO();
                artDao.insertArticulo(art);
                //artDao.GetArticulo()
                //HistorialArticulo ha = new HistorialArticulo(art.Code, DateTime.Now, art.Price, art.Status);
                //haDao.InsertHistorialArticulo(ha);
                MessageBox.Show("Insertado");
                MainWindow.StaticMainFrame.Content = new ListPage();
            }
        }

        private void txtUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUrl.Text.Length == 0) btnBuscar.IsEnabled = false;
            else btnBuscar.IsEnabled = true;
        }
    }
}
