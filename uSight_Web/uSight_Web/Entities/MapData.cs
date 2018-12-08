using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSight_Web.Entities
{
    public class MapMarker
    {
        public double lat;
        public double lng;
        public string desc;
    }

    public class MapData
    {
        public IEnumerable<MapMarker> markers;

        public MapData(List<MapMarker> markers)
        {
            this.markers = markers;
        }

        public string GetJSON()
        {
            var all = new JArray();
            foreach (var m in markers)
            {
                var x = new JObject();
                x.Add("lat", new JValue(m.lat));
                x.Add("lng", new JValue(m.lng));
                x.Add("desc", new JValue(m.desc));
                all.Add(x);
            }
            return all.ToString();
        }
    }
}