using System;
using System.Collections;
using System.Windows.Forms;

namespace Motley_Vis
{
    public partial class DataGridViewVirtual : Form
    {
        private DataGridView mainGridView = new DataGridView();

        private DataRowProvider datarows;

        [STAThreadAttribute()]
        public static void Main()
        {
            Application.Run(new DataGridViewVirtualized());
        }

        public DataGridViewVirtual()
        {
            InitializeComponent();

            mainGridView.Dock = DockStyle.Fill;
            Controls.Add(mainGridView);
            Load += new EventHandler(DataGridViewVirtual_Load);
        }

        private void DataGridViewVirtual_Load(object sender, EventArgs e)
        {
            // Enable virtual-mode
            mainGridView.VirtualMode = true;

            mainGridView.ReadOnly = true;
            mainGridView.AllowUserToAddRows = false;
            mainGridView.AllowUserToOrderColumns = false;

            mainGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            mainGridView.CellValueNeeded +=
                new DataGridViewCellValueEventHandler(mainGridView_CellValueNeeded);

            // Add columns to the DataGridView
            // var a = new DataGridViewTextBoxColumn();
            // this.mainGridView.Columns.Add(a);

            // Add data to the data store
        }

        private void mainGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // e.Value = row[e.RowIndex].column
            throw new NotImplementedException();
        }
    }
}
