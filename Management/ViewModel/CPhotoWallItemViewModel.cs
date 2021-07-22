using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CPhotoWallItemViewModel
    {

        public int MemberId { get; set; }
        public int PictureId { get; set; }
        public string Picture { get; set; }
        public bool Display { get; set; }
        public int CategoryId { get; set; }
        public string MemberAccountName { get; set; }
        public string Category { get; set; }
      

        public string ContributeTime { get; set; }
       
        public int CounttheSame { get; set; }
    }
}
