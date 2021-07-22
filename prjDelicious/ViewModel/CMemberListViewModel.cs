using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CMemberListViewModel
    {
        public Tmember member { get; set; }
        [DisplayName("會員序號")]
        public int memberId
        { get; set; }
        [DisplayName("會員姓名")]
        public string memberName
        { get; set; }
        [DisplayName("會員暱稱")]
        public string nickName
        { get; set; }
        [DisplayName("會員資訊")]
        public string info
        { get; set; }
    }
}
