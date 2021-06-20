using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLScraper
{
    public class Categ
    {
        private string _name;
        private string _url;
        public string Url { get => _url; set => _url = value; }
        public string Name { get => _name; set => _name = value; }

        public Categ(string name, string url)
        {
            _name = name;
            _url = url; 
        }
    }
}
