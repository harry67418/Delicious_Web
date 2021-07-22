using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class TphotoWallCategory
    {
        public TphotoWallCategory()
        {
            TphotoWalls = new HashSet<TphotoWall>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string HtmlClassName { get; set; }

        public virtual ICollection<TphotoWall> TphotoWalls { get; set; }
    }
}
