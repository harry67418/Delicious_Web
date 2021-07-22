using Microsoft.AspNetCore.Http;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CRecipeViewModel
    {
        public Tmember member { get; set; }
       
        public CIngredientRecordViewModel CIngredientRecord { get; set; }
        public CRecipeStepViewModel CRecipeStep { get; set; }

        public List<TmemberFollow> followeds { get; set; }

        public CRecipeViewModel()
        {
            list_ingredientRecords = new List<CIngredientRecordViewModel>();
            list_step = new List<CRecipeStepViewModel>();
            list_comment = new List<CCommentViewModel>();
            followeds = new List<TmemberFollow>();
        }

        public int recipeId { get; set; }

        [DisplayName("食譜名稱")]
        [Required(ErrorMessage = "食譜名稱不可空白")]
        [StringLength(20,MinimumLength = 2,ErrorMessage = "字數限制為2~20字)")]
        //[RegularExpression(@"^[\u4e00-\u9fa5_a-zA-Z]+$", ErrorMessage = "僅限中英文輸入(2~20字)")]
        public string recipeName { get; set; }

        public int MemberId { get; set; }

        [DisplayName("食譜簡述")]
        [Required(ErrorMessage = "食譜簡述不可空白")]
        [StringLength(200, ErrorMessage = "字數限制為10~200字", MinimumLength = 10)]
        public string recipeDescription { get; set; }

        [DisplayName("作者")]
        public string Nickname { get; set; }

        public int recipelikeCount { get; set; } 

        public int followerCount { get; set; }

        public string MemberPicture { get; set; }

        [DisplayName("發表時間")]
        public DateTime postTime { get; set; }

        public int recipeCategoryId { get; set; }

        [DisplayName("食譜類別")]
        public string recipeCategory { get; set; }

        [DisplayName("份數(人)")]
        [Required(ErrorMessage = "份數不可空白")]
        [Range(1, 9999, ErrorMessage = "限阿拉伯數字填寫(0-9999)")]
        public int forHowmany { get; set; }

        [DisplayName("時間(分)")]
        [Required(ErrorMessage = "製作時間不可空白")]
        [Range(0,9999 ,ErrorMessage = "限阿拉伯數字填寫(0-9999)")]
        public int timeNeed { get; set; }
        

       
        public string recipePicture { get; set; }

        [DisplayName("分享連結")]
        //[RegularExpression(@"^(https?://)?(www.)?youtube.com/(watch\?v=|user/|channel/)[a-zA-Z0-9-]+/?$", ErrorMessage = "僅限YouTube來源網站")]
        [Url(ErrorMessage = "資料內容非網址格式")]
        public string recipeVideo { get; set; }

        public int views { get; set; }
       
        [Required(ErrorMessage = "食譜照片不可空白")]
        public IFormFile recipephoto { get; set; }

        
        public List<CIngredientRecordViewModel> list_ingredientRecords { get; set; }     

        public List<CRecipeStepViewModel> list_step { get; set; }

        public List<CCommentViewModel> list_comment { get; set; }

        public List<TlikeComment> list_likecomment { get; set; }

        public List<TcollectionFolder> list_Folders { get; set; }

        public string CommentPicsSrc { get; set; }

        public string FigureSrc { get; set; }
    }
}
