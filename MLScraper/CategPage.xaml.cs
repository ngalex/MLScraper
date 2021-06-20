using ClassLibrary.DAO;
using ClassLibrary.Model;
using HtmlAgilityPack;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MLScraper
{
    /// <summary>
    /// Lógica de interacción para CategPage.xaml
    /// </summary>
    public partial class CategPage : Page
    {
        public ArrayList Articulos;
        public ArrayList CategGeneral;
        public ArrayList SubCateg;
        public List<List<Categ>> CategItems;
        public string BaseUrl = "https://www.mercadolibre.com.ar";

        public CategPage()
        {
            InitializeComponent();
            cmbSubCateg.IsEnabled = false;
            cmbItemCateg.IsEnabled = false;
            GetCategGeneral();
        }

        public void GetCategGeneral()
        {
            HtmlWeb oWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8
            };
            HtmlDocument doc = oWeb.Load(BaseUrl);
            CategGeneral = new ArrayList();
            try
            {
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class=\"category-column\"]");
                foreach (HtmlNode node in nodes)
                {
                    HtmlNodeCollection nodes2 = node.SelectNodes("./a[@class='category']");
                    foreach (HtmlNode node2 in nodes2)
                    {
                        Categ categ = new Categ(node2.SelectSingleNode("./p").InnerText, node2.Attributes["href"].Value);
                        CategGeneral.Add(categ);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error categGeneral");
            }
            cmbCateg.ItemsSource = CategGeneral;
            cmbCateg.DisplayMemberPath = "Name";
            cmbCateg.SelectedValuePath = "Url";
        }

        public void GetSubCateg(string url)
        {
            HtmlWeb oWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8
            };
            HtmlDocument doc = oWeb.Load(url);
            SubCateg = new ArrayList();
            CategItems = new List<List<Categ>>();
            try
            {
                HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class=\"desktop__view-child\"]");
                foreach (HtmlAgilityPack.HtmlNode node in nodes)
                {
                    HtmlNode aux;
                    List<Categ> items = new List<Categ>();

                    aux = node.SelectSingleNode("./a");
                    if (aux != null)
                    {
                        Categ categ = new Categ(aux.SelectSingleNode("./div").InnerText, aux.Attributes["href"].Value);
                        SubCateg.Add(categ);
                        items.Add(categ);
                    }

                    HtmlNodeCollection nodes2 = node.SelectSingleNode("ul").ChildNodes;
                    foreach (HtmlNode x in nodes2)
                    {
                        //Console.WriteLine(x.SelectSingleNode("./a").SelectSingleNode("./div").InnerText);
                        items.Add(new Categ(x.SelectSingleNode("./a").SelectSingleNode("./div").InnerText, x.SelectSingleNode("./a").Attributes["href"].Value));
                    }
                    CategItems.Add(items);
                }
            }
            catch
            {
                MessageBox.Show("Error subCateg");
            }
            cmbSubCateg.ItemsSource = SubCateg;
            cmbSubCateg.DisplayMemberPath = "Name";
            cmbSubCateg.SelectedValuePath = "Url";
        }

        public void GetItemCateg(int idx)
        {
            cmbItemCateg.ItemsSource = CategItems[idx];
            cmbItemCateg.DisplayMemberPath = "Name";
            cmbItemCateg.SelectedValuePath = "Url";
        }

        private void cmbCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCateg.SelectedIndex != -1)
            {
                cmbSubCateg.IsEnabled = true;
                GetSubCateg(cmbCateg.SelectedValue.ToString());
            }
            else
            {
                cmbSubCateg.IsEnabled = false;
                cmbItemCateg.IsEnabled = false;
            }
        }

        private void cmbSubCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSubCateg.SelectedIndex != -1)
            {
                cmbItemCateg.IsEnabled = true;
                GetItemCateg(cmbSubCateg.SelectedIndex);
            }
            else
            {
                cmbItemCateg.IsEnabled = false;
            }
        }

        private void cmbItemCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbItemCateg.SelectedIndex != -1)
            {
                string url = cmbItemCateg.SelectedValue.ToString();
                HtmlWeb oWeb = new HtmlWeb();
                oWeb.OverrideEncoding = Encoding.UTF8;
                HtmlDocument doc = oWeb.Load(url);
                Articulos = new ArrayList();
                try
                {
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class=\"ui-search-result__wrapper\"]");
                    foreach (HtmlNode node in nodes)
                    {
                        string artUrl = "", artPrice = "", artName = "";
                        HtmlNode aux;
                        aux = node.SelectSingleNode(".//a");
                        if (aux != null) artUrl = aux.Attributes["href"].Value;

                        aux = node.SelectSingleNode(".//span[@class='price-tag-fraction']");
                        if (aux != null) artPrice = aux.InnerText;

                        aux = node.SelectSingleNode(".//h2");
                        if (aux != null) artName = aux.InnerText;

                        Articulos.Add(new DGArticulo(artName, float.Parse(artPrice), artUrl, false));
                    }
                    DGArt.ItemsSource = Articulos;
                }
                catch
                {
                    MessageBox.Show("Error SubItem");
                }
            }
            
        }

        private void btnAddSelected_Click(object sender, RoutedEventArgs e)
        {
            ArticuloDAO artDao = new ArticuloDAO();
            foreach (DGArticulo item in Articulos)
            {
                if (item.IsSelected) artDao.insertArticulo(new Articulo(item.Url));
            }
            MainWindow.StaticMainFrame.Content = new ListPage();
        }

        private void btnAddAll_Click(object sender, RoutedEventArgs e)
        {
            ArticuloDAO artDao = new ArticuloDAO();
            foreach (DGArticulo item in Articulos)
            {
                artDao.insertArticulo(new Articulo(item.Url));
            }
            MainWindow.StaticMainFrame.Content = new ListPage();
        }
    }
}
