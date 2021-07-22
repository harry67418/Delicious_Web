using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CPhotoWallViewModel
    {
        public CPhotoWallViewModel() {
            list = new List<CPhotoWallItemViewModel>();
            categorylist = new List<string>();
        }
        public List<CPhotoWallItemViewModel> list { get; set; }
        public List<string> categorylist { get; set; }
        public int categorycount { get; set; }//算有幾種種類
       
    }
}
