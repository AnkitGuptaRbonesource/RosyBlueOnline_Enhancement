using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class MemoTallyStockByRfidViewModel
    {
        public List<inventoryDetailsViewModel> InMemo { get; set; }
        public List<inventoryDetailsViewModel> InBox { get; set; }
        public List<inventoryDetailsViewModel> Final { get; set; }
    }
}
