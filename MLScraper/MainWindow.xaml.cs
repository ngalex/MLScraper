using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ClassLibrary.DAO;
using ClassLibrary.Model;

namespace MLScraper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame StaticMainFrame;
        public static ArrayList checking_articulos;
        private static ArticuloDAO artDao = new ArticuloDAO();
        private static HistorialArticuloDAO haDao = new HistorialArticuloDAO();
        private static System.Windows.Forms.NotifyIcon nIcon = new NotifyIcon();
        private int frecuencia;
        private FrecuencyDAO frecDao = new FrecuencyDAO();
        System.Timers.Timer aTimer;

        public MainWindow()
        {
            InitializeComponent();

            StaticMainFrame = MainFrame;

            StaticMainFrame.Content = new ListPage(); // Pagina por defecto

            // Intervalo de checkeo de estado
            aTimer = new System.Timers.Timer();

            fillCmb();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = frecuencia;
            aTimer.Enabled = true;

            // Primera checkeo de estado
            Task.Run(() => OnTimedEvent(null,null));
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Scraping: " + DateTime.Now);
            checking_articulos = artDao.GetArticulos();
            foreach (Articulo art in checking_articulos)
            {
                float oldPrice = art.Price;
                art.Scraper(art.Url);
                float diff = art.Price - oldPrice;
                if(diff > 0)
                {
                    notify("./Resources/Icons/web.ico", "Alerta de AUMENTO de precio (+" + Math.Round(diff / oldPrice * 100, 2).ToString() + "%)" +
                        "\n $ " + Math.Round(art.Price, 2), art.Name);
                } else if(diff < 0)
                {
                    notify("./Resources/Icons/web.ico", "Alerta de DESCUENTO (" + Math.Round(diff / oldPrice * 100, 2).ToString() + "%)" +
                        "\n $ " + Math.Round(art.Price, 2), art.Name);
                }
                artDao.updateArticulo(art);
                HistorialArticulo ha = new HistorialArticulo(art.Code, DateTime.Now, art.Price, art.Status);
                haDao.InsertHistorialArticulo(ha);
            }
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if(StaticMainFrame.Content.ToString() == "MLScraper.ListPage")
                {
                    StaticMainFrame.Content = null;
                    StaticMainFrame.Content = new ListPage();
                }
            }));
        }

        private void openListPage(object sender, RoutedEventArgs e)
        {
            StaticMainFrame.Content = new ListPage();
        }

        private void openPageInsert(object sender, RoutedEventArgs e)
        {
            StaticMainFrame.Content = new PageInsert();
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            StaticMainFrame.Content = new PageNotifications();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //OnTimedEvent(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            notify("./Resources/Icons/web.ico","Contenido de Prueba", "Prueba");
        }

        public void notify(string icon, string text, string title)
        {
            try
            {
                nIcon.Icon = new Icon(icon);
                nIcon.Text = text;
                nIcon.Visible = true;
                nIcon.BalloonTipTitle = title;
                nIcon.BalloonTipText = text;
                nIcon.ShowBalloonTip(5000);
            } catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            
        }

        private void cmbFrec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frecuencia = int.Parse(cmbFrec.SelectedValue.ToString());
            aTimer.Interval = frecuencia;
            frecDao.UpdateFrecuency(cmbFrec.SelectedIndex+1);
        }

        private void fillCmb()
        {
            cmbFrec.ItemsSource = frecDao.GetFrecuency().DefaultView;
            cmbFrec.DisplayMemberPath = "name";
            cmbFrec.SelectedValuePath = "value";
            foreach (DataRowView item in cmbFrec.ItemsSource)
            {
                if (item["selected"].ToString() == "1")
                {
                    cmbFrec.SelectedValue = item["value"];
                    break;
                }
            }
            frecuencia = int.Parse(cmbFrec.SelectedValue.ToString());
        }

        private void btnCateg_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CategPage();
        }
    }
}
