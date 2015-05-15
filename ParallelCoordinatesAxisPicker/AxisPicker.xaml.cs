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

namespace ParallelCoordinatesAxisPicker
{
    /// <summary>
    /// Interaction logic for AxisPickerWindow.xaml
    /// </summary>
    public partial class AxisPickerWindow : Window
    {

        public AxisPickerWindow(IEnumerable<string> axisList)
        {
            InitializeComponent();

            foreach (var item in axisList)
            {
                FirstBox.Items.Add(item);
                SecondBox.Items.Add(item);
            }

            FirstBox.SelectedIndex = 0;
            SecondBox.SelectedIndex = 1;
        }

        public Tuple<int, int> SelectedIndexes { get; private set; }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndexes = new Tuple<int, int>(FirstBox.SelectedIndex, SecondBox.SelectedIndex);
            DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
