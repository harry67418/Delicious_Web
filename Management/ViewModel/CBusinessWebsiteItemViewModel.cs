using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CBusinessWebsiteItemViewModel
    {
        public int ingredientId { get; set; }
        public string ingredient { get; set; }
        public int thismonthsum { get; set; }
        public int lastmonthsum { get; set; }
        public int quantity { get; set; }
    }
}
