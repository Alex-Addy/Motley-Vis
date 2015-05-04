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
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                foreach (var header in datarows.Headers)
                {
                    comboBox1.Items.Add(header);
                    comboBox2.Items.Add(header);
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
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
                var wpfPCWindow = new ParallelCoordinates.ParallelCoordinatesWindow(measures, datarows.Headers);
                wpfPCWindow.Show();
            }
            else
            {
                MessageBox.Show(Resources.DataGridViewVirtual_No_Data_Loaded_Error);
            }
        }

        private void load2dBut_MouseClick(object sender, MouseEventArgs e)
        {
            int index1 = comboBox1.SelectedIndex;
            int index2 = comboBox2.SelectedIndex;

            var window = new _2D_Data_Graph.GraphView(datarows.FileName, comboBox1.SelectedText, comboBox2.SelectedText,
                datarows.GetEnumerable().Select(r =>
                {
                    double res; double.TryParse(r[index1], out res); return res;
                }),
                datarows.GetEnumerable().Select(r =>
                {
                    double res; double.TryParse(r[index2], out res); return res;
                }));
            window.Show();
        }
    }
}
