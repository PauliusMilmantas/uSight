using Emgu.CV.CvEnum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace uSight
{
    public partial class StatisticsForm : Form
    {
        ImageSource currentImageSource;


        public StatisticsForm(ImageSource imgSource)
        {
            currentImageSource = imgSource;
            InitializeComponent();
            StatisticsForm_Load(null, null);
        }

        private dynamic GetJsonFromDisk()
        {
            dynamic tmpObj;
            try
            {
                string data = File.ReadAllText("stats.json");
                tmpObj = JObject.Parse(data);
            }
            catch (Exception e)
            {
                tmpObj = new JObject();
            }


            return tmpObj;
        }

        private void writeToJson(string json)
        {
            System.IO.File.WriteAllText("stats.json", json);
        }

        private void StatisticsForm_Load(object sender, EventArgs e)
        {
            UtilFunctions f = new UtilFunctions(null);
            dynamic statsJson = GetJsonFromDisk();
            DataExtraction de = new DataExtraction();
            dynamic wantedJson = de.GetJsonFromDisk();

            if (currentImageSource != null)
            {
                DateTime date = currentImageSource.Date;
                for (int frame = 0; frame < currentImageSource.Count; frame++)
                {
                    List<string> numbers = f.ProcessImage(UtilFunctions.GetMatFromImage(currentImageSource[frame].Bitmap).GetUMat(AccessType.ReadWrite));
                    foreach (string number in numbers)
                    {
                        bool found = false;
                        foreach (var obj in statsJson.plates)
                        {
                            if (obj.time == date && obj.number == number)
                            {
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            found = false;
                            foreach (var obj in wantedJson.plates)
                            {
                                if (obj.p_number == number)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            JObject plate = new JObject();
                            plate["time"] = date;
                            plate["number"] = number;
                            plate["stolen"] = found;
                            statsJson.plates.Add(plate);
                        }
                    }
                }
                writeToJson(statsJson.ToString());
            }

            allStolenChart.Series.Clear();
            thisSourceStolenChart.Series.Clear();
            allBreakdownChart.Series.Clear();
            thisSourceBreakdownChart.Series.Clear();

            Series allTimeAll = new Series
            {
                Name = "All",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            Series allTimeStolen = new Series
            {
                Name = "Stolen",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            Series thisTimeAll = new Series
            {
                Name = "All",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            Series thisTimeStolen = new Series
            {
                Name = "Stolen",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            Series allTimeBreakdown = new Series
            {
                Name = "Count",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = false,
                ChartType = SeriesChartType.Bar
            };

            Series thisTimeBreakdown = new Series
            {
                Name = "Count",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Bar
            };
        }
    }
}
