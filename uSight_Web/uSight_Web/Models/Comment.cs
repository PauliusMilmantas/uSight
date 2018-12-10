using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace uSight_Web.Models
{
    public class Comment
    {
       
        [Key]
        [Column(Order = 1)]
        public string PlateNumber { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime Time { get; set; }
        [Key]
        [Column(Order = 3)]
        public string UserId { get; set; }
        public string Text { get; set; }
    }
}