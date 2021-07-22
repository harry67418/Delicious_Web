using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class TmerchandisePicture
    {
        public int MerchandisePicId { get; set; }
        public int IngredientId { get; set; }
        public string MerchandisePicture { get; set; }

        public virtual Tingredient Ingredient { get; set; }
    }
}
