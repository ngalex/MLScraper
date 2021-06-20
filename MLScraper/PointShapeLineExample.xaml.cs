using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using ClassLibrary.Model;
namespace Wpf.CartesianChart.PointShapeLine
{
    public partial class PointShapeLineExample : UserControl
    {
        public ArrayList historial = new ArrayList();
        public PointShapeLineExample()
        {
            InitializeComponent();

            
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ChartValues<float> prices = new ChartValues<float>();
            ArrayList lbls = new ArrayList();
            //historial.Reverse();
            int i = 30;
            foreach (HistorialArticulo item in historial)
            {
                if (i > 0)
                {
                    prices.Insert(0,item.Price);
                    lbls.Insert(0,item.Dt.ToString());
                    i--;
                }
                else break;
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "S(x)",

                    //Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                    Values = prices
                }
                /*new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }*/
            };

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            Labels = (string[])lbls.ToArray(typeof(string));
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            /*SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });*/

            //modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }
    }
}