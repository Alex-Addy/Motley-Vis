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
using System.Windows.Shapes;
using HelixToolkit;
using HelixToolkit.Wpf;

namespace _3d_Data_View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IEnumerable<List<double>> rows, int xAxis, int yAxis, int zAxis)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(rows, xAxis, yAxis, zAxis);
        }
    }
}
