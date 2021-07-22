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
    public class CRecipeStepViewModel
    {

        public int stepNumber { get; set; }

        [Required(ErrorMessage = "步驟內容不可空白")]
        [StringLength(200, ErrorMessage = "字數限制為1~200字", MinimumLength = 1)]
        public string step { get; set; }

        public string stepPicture { get; set; }

        public IFormFile stepphoto { get; set; }

    }
}
