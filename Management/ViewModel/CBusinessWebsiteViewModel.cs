using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CBusinessWebsiteViewModel
    {
        public List<CBusinessWebsiteItemViewModel> _MerchadiseRanklist { get; set; }
        public CBusinessWebsiteViewModel()
        {
            _MerchadiseRanklist = new List<CBusinessWebsiteItemViewModel>();
        }

    }
}
