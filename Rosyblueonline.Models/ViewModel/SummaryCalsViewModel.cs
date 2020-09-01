using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
   public class SummaryCalsViewModel
    {
        public String TotalCarat { get; set; }
        public string AvgRapPerCT { get; set; }
        public string TotalRap { get; set; }
        public string AvgRapoff { get; set; }
        public string PricePerct { get; set; }
        public string PayableAmount { get; set; }
        public String TotalPcs { get; set; }
    }
}
