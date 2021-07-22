using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TcollectionFolder
    {
        public TcollectionFolder()
        {
            Tcollections = new HashSet<Tcollection>();
        }

        public int CollectionFolderId { get; set; }
        public string CollectionFolder { get; set; }
        public int MemberId { get; set; }

        public virtual Tmember Member { get; set; }
        public virtual ICollection<Tcollection> Tcollections { get; set; }
    }
}
