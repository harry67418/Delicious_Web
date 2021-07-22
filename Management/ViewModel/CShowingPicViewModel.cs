using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CShowingPicViewModel
    {
        [DisplayName("輪播牆編號")]
        public int ShowingPicsId { get; set; }
        [DisplayName("輪播牆圖片標題")]
        public string ShowingPicsTitle { get; set; }
        [DisplayName("輪播牆圖片路徑")]
        public string ShowingPicsPathRoad { get; set; }
        [DisplayName("輪播牆圖片描述")]
        public string ShowingPicsDescription { get; set; }
        [DisplayName("輪播牆圖片外部連結")]
        public string ShowingPicsHyperLink { get; set; }
        [DisplayName("輪播牆圖片是否播放")]
        public bool ShowingPicsIsShowOrNot { get; set; }
        [DisplayName("輪播牆圖片上傳")]
        public IFormFile uploadpic { get; set; }

        public int counttrue { get; set; } 

    }
}
