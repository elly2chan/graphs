using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphSharp.Controls;
using GraphSharp;
using QuickGraph;
using EdgeColoring;
using System.IO;

namespace ViewGraph
{
    public partial class MainWindow : Window
    {
        public IBidirectionalGraph<object, IEdge<object>> Graph { get; set; }

        public MainWindow()
        {
            var graphIO = new GraphIO();
            var g = new UndirectedGraph<int, TaggedEdge<int,int>>();
            try
            {
                g = graphIO.readGraph(@"test.dot");
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("File not found");
            }

            if (g != null)
            {
                
                var ep = new EdgePainter();
                ep.edgeColoring(g);
                graphIO.writeToFile(@"ans.dot", g);
                //var converter = new GraphConverter();
                //var graph = converter.Convert(g);

                //Graph = graph;
            }
            InitializeComponent();
        }
    }

    public class MyEdge : TypedEdge<Object>
    {
        public String Id { get; set; }
        public Color EdgeColor { get; set; }
        public MyEdge(Object source, Object target) : base(source, target, EdgeTypes.General) { }
    }

    public class EdgeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
