namespace _2D_Data_Graph
{
    partial class GraphView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.generateButton = new System.Windows.Forms.Button();
            this.graphChoice = new System.Windows.Forms.ComboBox();
            this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(139, 12);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Generate Graph";
            this.generateButton.UseVisualStyleBackColor = true;
            // 
            // graphChoice
            // 
            this.graphChoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.graphChoice.FormattingEnabled = true;
            this.graphChoice.Location = new System.Drawing.Point(12, 12);
            this.graphChoice.Name = "graphChoice";
            this.graphChoice.Size = new System.Drawing.Size(121, 21);
            this.graphChoice.TabIndex = 1;
            // 
            // mainChart
            // 
            chartArea3.Name = "ChartArea1";
            this.mainChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.mainChart.Legends.Add(legend3);
            this.mainChart.Location = new System.Drawing.Point(13, 40);
            this.mainChart.Name = "mainChart";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.mainChart.Series.Add(series3);
            this.mainChart.Size = new System.Drawing.Size(970, 623);
            this.mainChart.TabIndex = 2;
            this.mainChart.Text = "chart1";
            // 
            // GraphView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 675);
            this.Controls.Add(this.mainChart);
            this.Controls.Add(this.graphChoice);
            this.Controls.Add(this.generateButton);
            this.Name = "GraphView";
            this.Text = "Graph View";
            this.Load += new System.EventHandler(this.GraphView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.ComboBox graphChoice;
        private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
    }
}

