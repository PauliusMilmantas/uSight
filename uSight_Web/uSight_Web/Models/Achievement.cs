using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace uSight_Web.Models
{
    public class Achievement
    {
        [Key, Column(Order = 1)]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("AchievementGroups")]
        public string GroupName { get; set; }
        [ForeignKey("AchievementGroups"), Column(Order = 3)]
        public int Tier { get; set; }
    }
}