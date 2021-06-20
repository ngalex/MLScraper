using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Model
{
    public class HistorialArticulo
    {
        private int _id;
        private string _code;
        private DateTime _dt;
        private float _price;
        private string _status;

        public int Id { get => _id; set => _id = value; }
        public string Code { get => _code; set => _code = value; }
        public DateTime Dt { get => _dt; set => _dt = value; }
        public float Price { get => _price; set => _price = value; }
        public string Status { get => _status; set => _status = value; }

        public HistorialArticulo() { }

        public HistorialArticulo(string code, DateTime dt, float price, string status)
        {
            _code = code;
            _dt = dt;
            _price = price;
            _status = status;
        }
    }
}
