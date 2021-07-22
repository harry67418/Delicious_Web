using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CVAccusationcs
    {
        public int AccusationRightId { get; set; }
        public int MemberId { get; set; }
        public string Accusation { get; set; }
        public string AccusedAvatar { get; set; }
        public string AccusedName { get; set; }
        public string ProgressStatu { get; set; }

        public string DisVisible { get; set; }
    }
}
