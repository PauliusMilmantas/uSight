using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace uSight_Web.Models
{
    public class SearchRecord
    {
        [Key]
        [Column(Order = 1)]
        public DateTime Time { get; set; }
        [Key]
        [Column(Order = 2)]
        public string PlateNumber { get; set; }
        public bool Stolen { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}