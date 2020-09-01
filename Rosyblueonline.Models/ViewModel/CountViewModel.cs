using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    // proc_cartWatch
    public class CartCountViewModel
    {
        public int CartExist { get; set; }
        public int RecentCartCount { get; set; }
        public int TotalCartCount { get; set; }
    }

    public class WatchListCountViewModel
    {
        public int WatchExist { get; set; }
        public int RecentWatchCount { get; set; }
        public int TotalWatchCount { get; set; }
    }

    // proc_CustomSiteSearch 'SpecificSearchCount'
    public class InventoryCountViewModel
    {
        public string countName { get; set; }
        public int ResultCount { get; set; }
    }
}
