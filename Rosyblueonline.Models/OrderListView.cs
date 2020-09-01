using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class OrderListView
    {
        [Key]
        public int orderDetailsId { get; set; }
        public DateTime orderCreatedOn { get; set; }
        public int orderStatus { get; set; }
        public double? orderPayableAmount { get; set; }
        public string companyName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string remark { get; set; }
        public int loginID { get; set; }
        public int orderType { get; set; }

        public int OrdCount { get; set; }
        public double? orderTotalCarat { get; set; }
        public double? orderPricePerCT { get; set; }
        public double? orderAmount { get; set; }
        public double? salesAmount { get; set; }
        public double? saleAvgDiscount { get; set; }
        public double? saleAvgRapPerCT { get; set; }
        public double? orderAvgDiscount { get; set; }
        public double? orderAvgRapPerCT { get; set; }
        public double? orderTotalRap { get; set; }
        public double? orderAvgRapOff { get; set; }
        public string CreatedName { get; set; }
        public int? createdBy { get; set; }
        public string ModifierName { get; set; }
        public int? modifiedBy { get; set; }
        public bool isSaleMode { get; set; }
    }
}
