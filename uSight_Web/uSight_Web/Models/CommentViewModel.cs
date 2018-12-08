using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSight_Web.Models
{
    public class CommentViewModel
    {
        public IEnumerable<uSight_Web.Models.SearchRecord> search { get; set; }
        public uSight_Web.Models.Comment comment { get; set; }
    }
}