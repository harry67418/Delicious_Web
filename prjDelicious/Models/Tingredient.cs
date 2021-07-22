using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class Tingredient
    {
        public Tingredient()
        {
            TingredientRecords = new HashSet<TingredientRecord>();
            TmerchandisePictures = new HashSet<TmerchandisePicture>();
            TorderDetails = new HashSet<TorderDetail>();
            TshoppingCarts = new HashSet<TshoppingCart>();
        }

        public int IngredientId { get; set; }
        public string Ingredient { get; set; }
        public int IngredientCategoryId { get; set; }
        public string IngredientUnit { get; set; }
        public decimal Price { get; set; }
        public int AmountInStore { get; set; }
        public string MerchandiseDescription { get; set; }
        public bool InStoreOrNot { get; set; }

        public virtual TingredientCategory IngredientCategory { get; set; }
        public virtual ICollection<TingredientRecord> TingredientRecords { get; set; }
        public virtual ICollection<TmerchandisePicture> TmerchandisePictures { get; set; }
        public virtual ICollection<TorderDetail> TorderDetails { get; set; }
        public virtual ICollection<TshoppingCart> TshoppingCarts { get; set; }
    }
}
