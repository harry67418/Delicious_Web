using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CAdminViewModel
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public bool AdminAuthority { get; set; }
    }
}
