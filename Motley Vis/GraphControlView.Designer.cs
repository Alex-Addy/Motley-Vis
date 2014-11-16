namespace Motley_Vis
{
    partial class GraphControlView
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
            this.graphGen2d = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.graphGen3d = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // graphGen2d
            // 
            this.graphGen2d.Location = new System.Drawing.Point(139, 37);
            this.graphGen2d.Name = "graphGen2d";
            this.graphGen2d.Size = new System.Drawing.Size(116, 23);
            this.graphGen2d.TabIndex = 0;
            this.graphGen2d.Text = "Generate 2D Graph";
            this.graphGen2d.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(12, 39);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 2;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(12, 66);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 3;
            // 
            // graphGen3d
            // 
            this.graphGen3d.Location = new System.Drawing.Point(139, 66);
            this.graphGen3d.Name = "graphGen3d";
            this.graphGen3d.Size = new System.Drawing.Size(116, 23);
            this.graphGen3d.TabIndex = 4;
            this.graphGen3d.Text = "Generate 3D Graph";
            this.graphGen3d.UseVisualStyleBackColor = true;
            // 
            // GraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 131);
            this.Controls.Add(this.graphGen3d);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.graphGen2d);
            this.Name = "GraphControl";
            this.Text = "GraphControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button graphGen2d;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button graphGen3d;
    }
}