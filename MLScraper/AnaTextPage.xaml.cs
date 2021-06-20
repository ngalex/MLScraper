using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MLScraper
{
    /// <summary>
    /// Lógica de interacción para AnaTextPage.xaml
    /// </summary>
    public partial class AnaTextPage : Page
    {
        public AnaTextPage(string url)
        {
            InitializeComponent();
            RemoveQueryStringByKey(url, "q");
        }

        public string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);
            if (uri.IsWellFormedOriginalString())
            {
                // this gets all the query string key value pairs as a collection
                var newQueryString = HttpUtility.ParseQueryString(uri.Query);

                // this removes the key if exists
                newQueryString.Remove(key);

                // this gets the page path from root without QueryString
                string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

                return newQueryString.Count > 0
                    ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                    : pagePathWithoutQueryString;

            }
            else return "";
        }
    }
}