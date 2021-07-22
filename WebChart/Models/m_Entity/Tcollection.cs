using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class Tcollection
    {
        public int CollectionId { get; set; }
        public int CollectionFolderId { get; set; }
        public int ReicipeId { get; set; }
        public DateTime Datetime { get; set; }

        public virtual TcollectionFolder CollectionFolder { get; set; }
        public virtual Trecipe Reicipe { get; set; }
    }
}
