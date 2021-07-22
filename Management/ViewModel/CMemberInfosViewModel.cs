using Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CMemberInfosViewModel
    {
        public CMemberInfosViewModel() {
            memberlist = new List<CMemberInfoItemsViewModel>();
        }
        public List<CMemberInfoItemsViewModel> memberlist { get; set; }
      

    }
}
