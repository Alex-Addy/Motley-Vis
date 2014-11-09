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
            Application.Run(new DataGridViewVirtual());
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
            mainGridView.RowHeadersVisible = false;

            mainGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            mainGridView.CellValueNeeded +=
                new DataGridViewCellValueEventHandler(mainGridView_CellValueNeeded);

            datarows = new DataRowProvider(@"E:\Test_Data\small_csv\people_5_col_with_headers.csv", new char[] { ',' });

            foreach (var header in datarows[0])
            {
                this.mainGridView.Columns.Add(new DataGridViewTextBoxColumn {HeaderText = header});
            }

            mainGridView.RowCount = datarows.Count;
        }

        private void mainGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = datarows[e.RowIndex][e.ColumnIndex];
        }
    }
}
