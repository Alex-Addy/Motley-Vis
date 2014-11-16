﻿using System;
using System.Collections;
using System.Windows.Forms;

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

            foreach (var header in datarows[0])
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
            }
        }
    }
}
