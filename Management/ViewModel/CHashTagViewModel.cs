using Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CHashTagViewModel
    {
        [DisplayName("標籤序號")]
        public int HashtagId { get; set; }
        [DisplayName("標籤名稱")]
        public string Hashtag { get; set; }
        [DisplayName("已使用食譜數")]
        public int TagRecipes { get; set; }
    }
}
