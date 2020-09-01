using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class OrderChargesViewModel
    {
        public int orderChargesId { get; set; }
        public int orderDetailsId { get; set; }
        public int chargesId { get; set; }
        public string chargesName { get; set; }
        public string chargesAbbrivation { get; set; }
        public int? chargesType { get; set; }
        public double? chargesValue { get; set; }
        public string actionMode { get; set; }
        public bool? isCappingApplied { get; set; }
        public double? minOrderValue { get; set; }
        public double? maxOrderValue { get; set; }
        private double? _chargesAmount = null;
        public double? chargesAmount
        {
            get
            {
                return _chargesAmount.HasValue ? Math.Round(_chargesAmount.Value, 2) : 0;
            }
            set { _chargesAmount = value; }
        }
        public int? customerId
        {
            get; set;
        }
        //public int chargesID { get; set; }
        //public string chargesName { get; set; }
        //public int chargesType { get; set; }
        //public double chargesValue { get; set; }
        //public string actionMode { get; set; }
        //public int minOrderValue { get; set; }
        //public int? maxOrderValue { get; set; }
        //public int? isCappingApplied { get; set; }
        //public double chargesAmount { get; set; }
    }
}
