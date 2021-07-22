using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class Tmember
    {
        public Tmember()
        {
            Taccusations = new HashSet<Taccusation>();
            TcollectionFolders = new HashSet<TcollectionFolder>();
            TcommentSections = new HashSet<TcommentSection>();
            Tfeedbacks = new HashSet<Tfeedback>();
            TlikeComments = new HashSet<TlikeComment>();
            TlikeRecipes = new HashSet<TlikeRecipe>();
            TmemberFollowFollowMembers = new HashSet<TmemberFollow>();
            TmemberFollowMembers = new HashSet<TmemberFollow>();
            Torders = new HashSet<Torder>();
            TphotoWalls = new HashSet<TphotoWall>();
            Trecipes = new HashSet<Trecipe>();
            TshoppingCarts = new HashSet<TshoppingCart>();
            TwishLists = new HashSet<TwishList>();
        }

        public int MemberId { get; set; }
        public string AccountName { get; set; }
        public string Nickname { get; set; }
        public string MemberName { get; set; }
        public string CellNumber { get; set; }
        public string CellConfirm { get; set; }
        public string Email { get; set; }
        public string EmailConfirm { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Info { get; set; }
        public bool ConfirmedOrNotEmail { get; set; }
        public bool ConfirmedOrNotPhone { get; set; }
        public string Password { get; set; }
        public DateTime RegisterTime { get; set; }
        public int PersonalRankScore { get; set; }
        public int RankId { get; set; }
        public string Figure { get; set; }
        public string GoogleId { get; set; }
        public bool FirstSetPassword { get; set; }

        public virtual TmemberRank Rank { get; set; }
        public virtual ICollection<Taccusation> Taccusations { get; set; }
        public virtual ICollection<TcollectionFolder> TcollectionFolders { get; set; }
        public virtual ICollection<TcommentSection> TcommentSections { get; set; }
        public virtual ICollection<Tfeedback> Tfeedbacks { get; set; }
        public virtual ICollection<TlikeComment> TlikeComments { get; set; }
        public virtual ICollection<TlikeRecipe> TlikeRecipes { get; set; }
        public virtual ICollection<TmemberFollow> TmemberFollowFollowMembers { get; set; }
        public virtual ICollection<TmemberFollow> TmemberFollowMembers { get; set; }
        public virtual ICollection<Torder> Torders { get; set; }
        public virtual ICollection<TphotoWall> TphotoWalls { get; set; }
        public virtual ICollection<Trecipe> Trecipes { get; set; }
        public virtual ICollection<TshoppingCart> TshoppingCarts { get; set; }
        public virtual ICollection<TwishList> TwishLists { get; set; }
    }
}
