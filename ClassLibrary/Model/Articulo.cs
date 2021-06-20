using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Linq;
using System.Text;

namespace ClassLibrary.Model
{
    public class Articulo
    {
        private string _code;
        private string _name;
        private string _url;
        private float _price = 0;
        private string _image;
        private string _status;
        private float _diff = 0;
        public string Image { get => _image; set => _image = value; }
        public string Name { get => _name; set => _name = value; }
        public float Price { get => _price; set => _price = value; }
        public string Code { get => _code; set => _code = value; }
        public string Url { get => _url; set => _url = value; }
        public string Status { get => _status; set => _status = value; }
        public float Diff { get => _diff; set => _diff = value; }

        public Articulo() {
            _code = "";
            _name = "";
            _url = "";
            _price = 0;
            _image = "";
            _status = ArticuloStatus.NO_ENCONTRADO.ToString();
            _diff = 0;
        }

        public Articulo(string url)
        {
            Scraper(url);
        }

        public Articulo(string code, string name, string url, float price, string image, string status)
        {
            _code = code;
            _name = name;
            _url = url;
            _price = price;
            _image = image;
            _status = status;
        }


        public void Scraper(string url)
        {
            Url = url;

            HtmlWeb oWeb = new HtmlWeb();
            oWeb.OverrideEncoding = Encoding.UTF8;
            HtmlDocument doc = oWeb.Load(url);

            try
            {
                HtmlNode hNode = doc.DocumentNode.SelectSingleNode("//div[@class='ui-pdp-price mt-16 ui-pdp-price--size-large']");
                if (hNode != null) hNode = hNode.SelectSingleNode("//div[@class='ui-pdp-price__second-line']");
                if (hNode != null) hNode = hNode.CssSelect(".price-tag-fraction").First();
                if (hNode != null) Price = float.Parse(hNode.InnerText);

                hNode = doc.DocumentNode.CssSelect(".ui-pdp-title").First();
                if (hNode != null) Name = hNode.InnerText;
                hNode = doc.DocumentNode.SelectSingleNode("//div[@class='ui-pdp-gallery']").CssSelect(".ui-pdp-gallery__figure__image").First();
                if (hNode != null) Image = hNode.Attributes["src"].Value.ToString();

                Status = ArticuloStatus.ONLINE.ToString();
            }
            catch
            {
                if (Status == ArticuloStatus.ONLINE.ToString())
                {
                    Console.WriteLine(Name + " : " + ArticuloStatus.OFFLINE.ToString());
                    Status = ArticuloStatus.OFFLINE.ToString();
                }
                else
                {
                    Console.WriteLine(Name + " : " + ArticuloStatus.NO_ENCONTRADO.ToString());
                    Status = ArticuloStatus.NO_ENCONTRADO.ToString();
                }

            }

            try
            {
                if (doc.DocumentNode.CssSelect(".andes-message__text--warning").First() != null)
                {
                    Status = ArticuloStatus.PAUSADA.ToString();
                }
            }
            catch { }
        }

            public float Difference(float inicial, float final)
        {
            return (float) Math.Round((final - inicial) / inicial * 100,2);
        }
    }
}
