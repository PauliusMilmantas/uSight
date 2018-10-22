namespace uSight
{
    partial class StatisticsForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.allStolenChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.thisSourceStolenChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.allBreakdownChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.thisSourceBreakdownChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.allStolenChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thisSourceStolenChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allBreakdownChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thisSourceBreakdownChart)).BeginInit();
            this.SuspendLayout();
            // 
            // allStolenChart
            // 
            chartArea1.Name = "ChartArea1";
            this.allStolenChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.allStolenChart.Legends.Add(legend1);
            this.allStolenChart.Location = new System.Drawing.Point(12, 12);
            this.allStolenChart.Name = "allStolenChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.allStolenChart.Series.Add(series1);
            this.allStolenChart.Size = new System.Drawing.Size(394, 298);
            this.allStolenChart.TabIndex = 0;
            this.allStolenChart.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "All Time";
            this.allStolenChart.Titles.Add(title1);
            // 
            // thisSourceStolenChart
            // 
            chartArea2.Name = "ChartArea1";
            this.thisSourceStolenChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.thisSourceStolenChart.Legends.Add(legend2);
            this.thisSourceStolenChart.Location = new System.Drawing.Point(445, 13);
            this.thisSourceStolenChart.Name = "thisSourceStolenChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.thisSourceStolenChart.Series.Add(series2);
            this.thisSourceStolenChart.Size = new System.Drawing.Size(409, 297);
            this.thisSourceStolenChart.TabIndex = 1;
            this.thisSourceStolenChart.Text = "chart1";
            title2.Name = "Title1";
            title2.Text = "This Media";
            this.thisSourceStolenChart.Titles.Add(title2);
            // 
            // allBreakdownChart
            // 
            chartArea3.Name = "ChartArea1";
            this.allBreakdownChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.allBreakdownChart.Legends.Add(legend3);
            this.allBreakdownChart.Location = new System.Drawing.Point(12, 325);
            this.allBreakdownChart.Name = "allBreakdownChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.allBreakdownChart.Series.Add(series3);
            this.allBreakdownChart.Size = new System.Drawing.Size(842, 237);
            this.allBreakdownChart.TabIndex = 2;
            this.allBreakdownChart.Text = "chart1";
            title3.Name = "Title1";
            title3.Text = "All Time";
            this.allBreakdownChart.Titles.Add(title3);
            // 
            // thisSourceBreakdownChart
            // 
            chartArea4.Name = "ChartArea1";
            this.thisSourceBreakdownChart.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.thisSourceBreakdownChart.Legends.Add(legend4);
            this.thisSourceBreakdownChart.Location = new System.Drawing.Point(12, 582);
            this.thisSourceBreakdownChart.Name = "thisSourceBreakdownChart";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.thisSourceBreakdownChart.Series.Add(series4);
            this.thisSourceBreakdownChart.Size = new System.Drawing.Size(842, 234);
            this.thisSourceBreakdownChart.TabIndex = 3;
            this.thisSourceBreakdownChart.Text = "thisSourceBreakdownChart";
            title4.Name = "Title1";
            title4.Text = "This Media";
            this.thisSourceBreakdownChart.Titles.Add(title4);
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 888);
            this.Controls.Add(this.thisSourceBreakdownChart);
            this.Controls.Add(this.allBreakdownChart);
            this.Controls.Add(this.thisSourceStolenChart);
            this.Controls.Add(this.allStolenChart);
            this.Name = "StatisticsForm";
            this.Text = "Statistics";
            ((System.ComponentModel.ISupportInitialize)(this.allStolenChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thisSourceStolenChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allBreakdownChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thisSourceBreakdownChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart allStolenChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart thisSourceStolenChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart allBreakdownChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart thisSourceBreakdownChart;
    }
}