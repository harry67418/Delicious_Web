using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CHomePageViewModel
    {
        public IEnumerable<CPhotoWallViewModel> photowalls { get; set; }
        public IEnumerable<Tmember> members { get; set; }
        public IEnumerable<TshowingPic> showpics { get; set; }

        public string PhotoWallSrc { get; set; }
        public string ShowPicsSrc { get; set; }
        public string FigureSrc { get; set; }
    }
}
