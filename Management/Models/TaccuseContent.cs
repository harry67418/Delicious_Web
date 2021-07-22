using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TaccuseContent
    {
        public TaccuseContent()
        {
            Taccusations = new HashSet<Taccusation>();
        }

        public int AccuseId { get; set; }
        public string Accusation { get; set; }

        public virtual ICollection<Taccusation> Taccusations { get; set; }
    }
}
