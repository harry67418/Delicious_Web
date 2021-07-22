using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using prjDelicious.Models;

namespace prjDelicious.ViewModel
{
    public class CIngredientRecordViewModel
    {

        [DisplayName("食材")]
        public string ingredient { get ; set; }

        public int ingredientCategoryId { get; set; }

        public int ingredientId { get; set; }

        [DisplayName("單位")]
        [Required(ErrorMessage = " 單位不可空白 ")]
        [StringLength(10, ErrorMessage = " 僅限中英文輸入 ", MinimumLength = 1)]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z]+$", ErrorMessage = "僅限中英文輸入")]
        public string unt { get; set; }


        [DisplayName("數量")]
        [Required(ErrorMessage = " 數量不可空白 ")]
        [Range(0, 9999, ErrorMessage = " 僅能輸入數字 ")]
        public double ingredientAmountForUse { get; set; }

    }
}
