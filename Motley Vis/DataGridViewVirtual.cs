using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Motley_Vis.Properties;

namespace Motley_Vis
{
    public partial class DataGridViewVirtual : Form
    {
        private DataRowProvider datarows;

        [STAThreadAttribute()]
        public static void Main()
        {
            Application.Run(new DataGridViewVirtual());
        }

        public DataGridViewVirtual()
        {
            InitializeComponent();

            // This needs to happen at some point after initialization and before the datagrid is used
            dataGridView1.CellValueNeeded +=
                new DataGridViewCellValueEventHandler(DataGridView_CellValueNeeded);
        }

        private void DataGridViewVirtual_Load(string filename)
        {
            dataGridView1.Columns.Clear();
            datarows = new DataRowProvider(filename, new char[] { '\t', ',' });

            foreach (var header in datarows.Headers)
            {
                this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn {HeaderText = header});
            }

            dataGridView1.RowCount = datarows.Count;
        }

        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = datarows[e.RowIndex][e.ColumnIndex];
        }

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            var selectDialog = new OpenFileDialog {Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*"};
            DialogResult result = selectDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                DataGridViewVirtual_Load(selectDialog.FileName);
                foreach (var header in datarows.Headers)
                {
                    comboBox1.Items.Add(header);
                    comboBox2.Items.Add(header);
                    comboBox3.Items.Add(header);
                }
            }
        }

        private void load3dBut_MouseClick(object sender, MouseEventArgs e)
        {

            if (datarows != null)
            {
                int index1 = comboBox1.SelectedIndex;
                int index2 = comboBox2.SelectedIndex;
                int index3 = comboBox3.SelectedIndex;

                IEnumerable<List<double>> points = datarows.GetEnumerable().
                    Select(r => new List<string> {r[index1], r[index2], r[index3]}.
                        ConvertAll(str => {
                                              double res;
                                              double.TryParse(str, out res);
                                              return res;
                        })
                    );

                var wpf3DWindow = new _3d_Data_View.MainView(points, 0, 1, 2);
                ElementHost.EnableModelessKeyboardInterop(wpf3DWindow);
                wpf3DWindow.Show();
            }
            else
            {
                MessageBox.Show(Resources.DataGridViewVirtual_No_Data_Loaded_Error);
            }
        }

        private void pc_launch_Click(object sender, EventArgs e)
        {
            if (datarows != null)
            {
                IEnumerable<List<double>> measures = datarows.GetEnumerable().
                    Select(r => r.ConvertAll(str =>
                    {
                        double res;
                        double.TryParse(str, out res);
                        return res;
                    })
                    );
                var wpfPCWindow = new ParallelCoordinates.MainWindow(measures, datarows.Headers);
                wpfPCWindow.Show();
            }
            else
            {
                MessageBox.Show(Resources.DataGridViewVirtual_No_Data_Loaded_Error);
            }
        }
    }
}
