using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TphotoWall
    {
        public int MemberId { get; set; }
        public int PictureId { get; set; }
        public string Picture { get; set; }
        public DateTime ContributeTime { get; set; }
        public bool Display { get; set; }
        public int CategoryId { get; set; }

        public virtual TphotoWallCategory Category { get; set; }
        public virtual Tmember Member { get; set; }
    }
}
