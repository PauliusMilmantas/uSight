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
            catch (Exception)
            {
                tmpObj = new JObject();
                tmpObj.Add("plates", new JArray());
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

            dynamic thisJson = null;

            if (currentImageSource != null)
            {
                thisJson = new JObject();
                thisJson.Add("plates", new JArray());
                DateTime date = currentImageSource.Date;

                var thisSourcePlatesQuery =
                    from img in currentImageSource
                    from number in f.ProcessImage(UtilFunctions.GetMatFromImage(img.Bitmap).GetUMat(AccessType.ReadWrite))
                    join wanted in (wantedJson as JObject)["plates"] on number equals wanted["p_number"].ToObject<string>() into g
                    select new { number, date, stolen = g.Any()};
                var thisSourcePlatesList = thisSourcePlatesQuery.Select(o => { JObject plate = new JObject(); plate["time"] = o.date; plate["number"] = o.number; plate["stolen"] = o.stolen; return plate; }).ToList();
                foreach (JObject plate in thisSourcePlatesList)
                {
                    thisJson.plates.Add(plate);
                    bool found = false;
                    foreach (var statsPlate in statsJson.plates)
                    {
                        if (statsPlate.time == plate["time"].ToObject<DateTime>() && statsPlate.number == plate["number"].ToObject<string>())
                        {
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        statsJson.plates.Add(plate);
                    }
                }

                writeToJson(statsJson.ToString());
            }
            statsJson = GetJsonFromDisk();

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
                ChartType = SeriesChartType.Column
            };

            Series thisTimeStolen = new Series
            {
                Name = "Stolen",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };

            Series allTimeBreakdown = new Series
            {
                Name = "Count",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };

            Series thisTimeBreakdown = new Series
            {
                Name = "Count",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };

            allStolenChart.Series.Add(allTimeAll);
            allStolenChart.Series.Add(allTimeStolen);
            thisSourceStolenChart.Series.Add(thisTimeAll);
            thisSourceStolenChart.Series.Add(thisTimeStolen);
            allBreakdownChart.Series.Add(allTimeBreakdown);
            thisSourceBreakdownChart.Series.Add(thisTimeBreakdown);

            var allTimeAllQuery =
                from plate in (statsJson as JObject)["plates"]
                group plate by plate["time"].ToObject<DateTime>() into p
                select new { time = p.Key, count = p.Count() };
            foreach(var x in allTimeAllQuery)
            {
                allTimeAll.Points.AddXY(x.time, x.count);
            }

            var allTimeStolenQuery =
                from plate in (statsJson as JObject)["plates"]
                where plate["stolen"].ToObject<bool>()
                group plate by plate["time"].ToObject<DateTime>() into p
                select new { time = p.Key, count = p.Count() };
            foreach (var x in allTimeStolenQuery)
            {
                allTimeStolen.Points.AddXY(x.time, x.count);
            }

            var allTimeBreakdownQuery =
                from plate in (statsJson as JObject)["plates"]
                group plate by plate["number"].ToObject<string>() into p
                select new { number = p.Key, count = p.Count() };
            foreach (var x in allTimeBreakdownQuery)
            {
                allTimeBreakdown.Points.AddXY(x.number, x.count);
            }

            if (thisJson != null)
            {
                var thisTimeAllQuery =
                    from plate in (thisJson as JObject)["plates"]
                    group plate by plate["time"].ToObject<DateTime>() into p
                    select new { time = p.Key, count = p.Count() };
                foreach (var x in thisTimeAllQuery)
                {
                    thisTimeAll.Points.AddXY(x.time, x.count);
                }

                var thisTimeStolenQuery =
                    from plate in (thisJson as JObject)["plates"]
                    where plate["stolen"].ToObject<bool>()
                    group plate by plate["time"].ToObject<DateTime>() into p
                    select new { time = p.Key, count = p.Count() };
                foreach (var x in thisTimeStolenQuery)
                {
                    thisTimeStolen.Points.AddXY(x.time, x.count);
                }

                var thisTimeBreakdownQuery =
                    from plate in (thisJson as JObject)["plates"]
                    group plate by plate["number"].ToObject<string>() into p
                    select new { number = p.Key, count = p.Count() };
                foreach (var x in thisTimeBreakdownQuery)
                {
                    thisTimeBreakdown.Points.AddXY(x.number, x.count);
                }
            }

            allStolenChart.ChartAreas[allTimeAll.ChartArea].AxisX.ScrollBar.Enabled = true;
            allStolenChart.ChartAreas[allTimeAll.ChartArea].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            allStolenChart.ChartAreas[allTimeAll.ChartArea].AxisX.ScaleView.Zoomable = false;
            allStolenChart.ChartAreas[allTimeAll.ChartArea].AxisX.ScaleView.Size = 4;

            thisSourceStolenChart.ChartAreas[thisTimeAll.ChartArea].AxisX.ScrollBar.Enabled = true;
            thisSourceStolenChart.ChartAreas[thisTimeAll.ChartArea].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            thisSourceStolenChart.ChartAreas[thisTimeAll.ChartArea].AxisX.ScaleView.Zoomable = false;
            thisSourceStolenChart.ChartAreas[thisTimeAll.ChartArea].AxisX.ScaleView.Size = 4;

            allBreakdownChart.ChartAreas[allTimeBreakdown.ChartArea].AxisX.ScrollBar.Enabled = true;
            allBreakdownChart.ChartAreas[allTimeBreakdown.ChartArea].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            allBreakdownChart.ChartAreas[allTimeBreakdown.ChartArea].AxisX.ScaleView.Zoomable = false;
            allBreakdownChart.ChartAreas[allTimeBreakdown.ChartArea].AxisX.ScaleView.Size = 4;

            thisSourceBreakdownChart.ChartAreas[thisTimeBreakdown.ChartArea].AxisX.ScrollBar.Enabled = true;
            thisSourceBreakdownChart.ChartAreas[thisTimeBreakdown.ChartArea].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            thisSourceBreakdownChart.ChartAreas[thisTimeBreakdown.ChartArea].AxisX.ScaleView.Zoomable = false;
            thisSourceBreakdownChart.ChartAreas[thisTimeBreakdown.ChartArea].AxisX.ScaleView.Size = 4;

            allStolenChart.Invalidate();
            thisSourceStolenChart.Invalidate();
            allBreakdownChart.Invalidate();
            thisSourceBreakdownChart.Invalidate();
        }
    }
}
