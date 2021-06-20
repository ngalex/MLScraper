using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLScraper
{
    public class DGArticulo
    {
        private string _name;
        private float _price;
        private string _url;
        private bool _isSelected;

        public DGArticulo (string name, float price, string url, bool selected)
        {
            _name = name;
            _price = price;
            _url = url;
            IsSelected = selected;
        }

        public bool IsSelected { get => _isSelected; set => _isSelected = value; }
        public string Name { get => _name; set => _name = value; }
        public float Price { get => _price; set => _price = value; }
        public string Url { get => _url; set => _url = value; }
    }
}
