using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParallelCoordinates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Line> axesList = new List<Line>();

        public MainWindow(IEnumerable<List<double>> rows, List<String> headers)
        {
            InitializeComponent();

            foreach (string header in headers)
            {
                var newLn = new Line { Stroke = Brushes.Black, StrokeThickness = 2 };
                axesList.Add(newLn);
                this.Canvas.Children.Add(newLn);
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            redrawAxis();
        }

        private void redrawAxis()
        {
            var spacing = Canvas.ActualWidth/(axesList.Count + 1);

            foreach (var i in Enumerable.Range(0, axesList.Count))
            {
                var axis = axesList[i];
                axis.Y1 = 30;
                axis.Y2 = this.Canvas.ActualHeight - 30;
                axis.X1 = spacing*(i+1);
                axis.X2 = spacing*(i+1);
            }
        }
    }
}
