using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TshowingPic
    {
        public int ShowingPicsId { get; set; }
        public string ShowingPicsTitle { get; set; }
        public string ShowingPicsPathRoad { get; set; }
        public string ShowingPicsDescription { get; set; }
        public string ShowingPicsHyperLink { get; set; }
        public bool ShowingPicsIsShowOrNot { get; set; }
    }
}
