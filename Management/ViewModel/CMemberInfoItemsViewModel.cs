using Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CMemberInfoItemsViewModel
    {
        public CMemberInfoItemsViewModel() {
            MemberInfo = new Tmember();
            trecipelist = new List<string>();
            ordertime = new List<string>();
            contributephotowalltime = new List<string>();
        }
        public Tmember MemberInfo { get; set; }
        public int MemberId { get { return MemberInfo.MemberId; } set { MemberInfo.MemberId = value; } }
        public string AccountName { get { return MemberInfo.AccountName; } set { MemberInfo.AccountName = value; } }
        public string Nickname { get { return MemberInfo.Nickname; } set { MemberInfo.Nickname = value; } }
        public string MemberName { get { return MemberInfo.MemberName; } set { MemberInfo.MemberName = value; } }
        public string CellNumber { get { return MemberInfo.CellNumber; } set { MemberInfo.CellNumber = value; } }
        public string CellConfirm { get { return MemberInfo.CellConfirm; } set { MemberInfo.CellConfirm = value; } }
        public string Email { get { return MemberInfo.Email; } set { MemberInfo.Email = value; } }
        [DisplayFormat(DataFormatString ="{0:d}")]
        public string  Birthday { get; set;  }
        public string Gender { get { return MemberInfo.Gender; } set { MemberInfo.Gender = value; } }
        public string Info { get { return MemberInfo.Info; } set { MemberInfo.Info = value; } }
        public bool ConfirmedOrNotEmail { get { return MemberInfo.ConfirmedOrNotEmail; } set { MemberInfo.ConfirmedOrNotEmail = value; } }
        public bool ConfirmedOrNotPhone { get { return MemberInfo.ConfirmedOrNotPhone; } set { MemberInfo.ConfirmedOrNotPhone = value; } }
        public string Password { get { return MemberInfo.Password; } set { MemberInfo.Password = value; } }
        public DateTime RegisterTime { get { return MemberInfo.RegisterTime; } set { MemberInfo.RegisterTime = value; } }
        public int PersonalRankScore { get { return MemberInfo.PersonalRankScore; } set { MemberInfo.PersonalRankScore = value; } }
        public int RankId { get { return MemberInfo.RankId; } set { MemberInfo.RankId = value; } }
        public string Figure { get { return MemberInfo.Figure; } set { MemberInfo.Figure = value; } }

        public int recipecount { get; set; }
        public int likerecipecount { get; set; }
        public int collectioncount { get; set; }
        public int ordercount { get; set; }
        public int commentcount { get; set; }

        public int contributephotowallcount { get; set; }

        public List<string> trecipelist { get; set; }

        public List<string> ordertime { get; set; }

        public List<string> contributephotowalltime { get; set; }

    }
}
