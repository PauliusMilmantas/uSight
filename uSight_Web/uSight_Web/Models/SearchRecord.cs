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
    }
}