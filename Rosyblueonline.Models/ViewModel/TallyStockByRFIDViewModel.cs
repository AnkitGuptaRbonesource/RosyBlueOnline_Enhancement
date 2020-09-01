using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class TallyStockViewModel
    {
        public List<TallyStockByRFIDViewModel> StockCount { get; set; }
        public List<inventoryDetailsViewModel> Inventory { get; set; }
    }
    public class TallyStockByRFIDViewModel
    {
        public int TotalCount { get; set; }
        public string Total { get; set; }
        public int ActiveStockCount { get; set; }
        public string ActiveCount { get; set; }
        public int OnMemoCount { get; set; }
        public string OnMemo { get; set; }
        public int SoldCount { get; set; }
        public string Sold { get; set; }
        public int OrderPendingCount { get; set; }
        public string OrderPending { get; set; }
        public int InvalidCount { get; set; }
        public string Invalid { get; set; }
        public int UnReferencedCount { get; set; }
        public string UnReferenced { get; set; }
        public string BoxName { get; set; }
    }
}
