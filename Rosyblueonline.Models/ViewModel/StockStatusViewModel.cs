using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class StockStatusViewModel
    {
        public List<inventoryDetailsViewModel> inventories { get; set; }
        public int TotalCount { get; set; }
    }
}
