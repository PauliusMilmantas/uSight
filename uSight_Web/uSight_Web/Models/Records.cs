using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSight_Web.Models
{
    public class Records
    {
        public int id { get; set; }
        public string license_plate { get; set; }
        public int times_searched { get; set; }
        public string latest_time { get; set; }
        public string searched_by { get; set; }
    }
}