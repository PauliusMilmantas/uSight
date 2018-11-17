using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace uSight_Web.Entities
{
    public class ChartSeries
    {
        public string name;
        public IEnumerable<int> data;
    }

    public class ChartData
    {
        public IEnumerable<string> labels;
        public IEnumerable<ChartSeries> series;

        public ChartData(IEnumerable<string> labels, IEnumerable<string> names, IEnumerable<IEnumerable<int>> data)
        {
            this.labels = labels;
            var series = new List<ChartSeries>();
            foreach (var n in names)
            {
                series.Add(new ChartSeries() { name = n });
            }
            int index = 0;
            foreach (var d in data)
            {
                series[index].data = d;
                index++;
            }
            this.series = series;
        }

        public string GetLabelsJSON()
        {
            var all = new JArray();
            foreach (var s in labels)
            {
                all.Add(s);
            }
            return all.ToString();
        }

        public string GetSeriesJSON()
        {
            var all = new JArray();
            foreach (var s in series)
            {
                var set = new JObject();
                set.Add("label", new JValue(s.name));
                var data = new JArray();
                foreach (var x in s.data)
                {
                    data.Add(new JValue(x));
                }
                set.Add("data", data);
                all.Add(set);
            }
            return all.ToString();
        }
    }
}