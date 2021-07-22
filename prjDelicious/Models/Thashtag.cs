using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class Thashtag
    {
        public Thashtag()
        {
            ThashtagRecords = new HashSet<ThashtagRecord>();
        }

        public int HashtagId { get; set; }
        public string Hasgtag { get; set; }

        public virtual ICollection<ThashtagRecord> ThashtagRecords { get; set; }
    }
}
