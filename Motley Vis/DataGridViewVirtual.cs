﻿// Copyright (c) 2015 Alexander Addy
// Released under MIT license,
// view License.txt in root of project for full text
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

        private DataGridViewVirtual()
        {
            InitializeComponent();

            // This needs to happen at some point after initialization and before the datagrid is used
            dataGridView1.CellValueNeeded +=
                new DataGridViewCellValueEventHandler(DataGridView_CellValueNeeded);
        }

        private void DataGridViewVirtual_Load(string filename)
        {
            dataGridView1.Columns.Clear();
            datarows = new DataRowProvider(filename, new[] { '\t', ',' });

            foreach (var header in datarows.Headers)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn {HeaderText = header});
            }

            dataGridView1.RowCount = datarows.Count;
        }

        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = datarows[e.RowIndex][e.ColumnIndex];
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

        private void parallelCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectDialog = new OpenFileDialog { Filter = Resources.Click_CSV_Files_All_Files };
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
    }
}
