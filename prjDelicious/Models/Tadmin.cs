using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class Tadmin
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public bool AdminAuthority { get; set; }
    }
}
