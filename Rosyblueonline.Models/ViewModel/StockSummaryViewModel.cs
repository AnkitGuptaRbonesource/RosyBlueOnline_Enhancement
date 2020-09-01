using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class StockSummaryViewModel
    {
        public string Sizes { get; set; }
        public int TotInv { get; set; }
        public int TotOnMemo { get; set; }
        public int TotOrdPending { get; set; }
        public int TotAvailbale { get; set; }

        public double TotInvValue { get; set; } //Added by Ankit 30JUn2020
        public double TotOnMemoValue { get; set; }
        public double TotOrdPendingValue { get; set; }
        public double TotAvailbaleValue { get; set; }

          
             
    }
    public class StoneDetailsStockSummaryModel
    {
        public string Color { get; set; }
        public string Clarity { get; set; }
        public string AvailbaleValue { get; set; } 



    }
}
