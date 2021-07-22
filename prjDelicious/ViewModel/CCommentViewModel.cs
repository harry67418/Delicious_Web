using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CCommentViewModel
    {
        public TcommentSection _comment { get; set; }
        public Tmember _member { get; set; }
        public CCommentViewModel()
        {
            _comment = new TcommentSection();
            _member = new Tmember();
        }

        public int likeCouint { get; set; }

    }
}
