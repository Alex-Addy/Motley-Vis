using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow(IEnumerable<List<double>> rows, List<String> headers)
        {
            InitializeComponent();

            var txt1 = new TextBlock {FontSize = 14, Text = "Hello World!"};
            Canvas.SetTop(txt1, 100);
            Canvas.SetLeft(txt1, 10);
            this.Canvas.Children.Add(txt1);
        }
    }
}
