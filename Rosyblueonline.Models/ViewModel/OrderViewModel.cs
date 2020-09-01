using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class OrderViewModel
    {
        public List<inventoryDetailsViewModel> Inventory { get; set; }
        public SummaryCalsViewModel Summary { get; set; }
        public List<OrderChargesViewModel> Charges { get; set; }
    }

    //Note: Used with memo also.

    public class MemoDetail
    {
        public List<InventoryUpload> Inv { get; set; }
        public PlaceOrderViewModel Counts { get; set; }
    }
    public class PlaceOrderViewModel
    {
        public int validCount { get; set; }
        public int InvalidCount { get; set; }
        public int OrderId { get; set; }
        
    }

}
