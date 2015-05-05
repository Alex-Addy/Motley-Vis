using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _2D_Data_Graph
{
    public partial class GraphView : Form
    {
        private readonly string title, axisXTitle, axisYTitle;
        private readonly List<double> xData, yData; 
        public GraphView(string title, string axisXTitle, string axisYTitle, IEnumerable<double> firstDoubles, IEnumerable<double> seconDoubles)
        {
            InitializeComponent();

            this.title = title;
            this.axisXTitle = axisXTitle;
            this.axisYTitle = axisYTitle;
            this.xData = firstDoubles.ToList();
            this.yData = seconDoubles.ToList();
            mainChart.Titles.Add(title);
            mainChart.Titles.Add(axisXTitle);
            mainChart.Titles.Add(axisYTitle);
        }

        private void GraphView_Load(object sender, EventArgs e)
        {
            var dpCol = mainChart.Series.First().Points;
            dpCol.AddXY(10, 10);
            dpCol[0].IsEmpty = true;
            for (int i = 0; i < xData.Count; i++)
            {
                dpCol.AddXY(xData[i], yData[i]);
            }
        }
    }
}
