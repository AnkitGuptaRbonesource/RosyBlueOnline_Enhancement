using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class orderDetailModel
    {
        [Key]
        public int orderDetailsId { get; set; }
        public int createdBy { get; set; }
        public int customerId { get; set; }
        public DateTime orderDate { get; set; }
        public double orderPricePerCT { get; set; }
        public double orderTotalCarat { get; set; }
        public double orderAmount { get; set; }
        public double orderPayableAmount { get; set; }
        public double orderAvgDiscount { get; set; }
        public double orderTotalExDiscount{ get; set; }
        public double orderTotalExCharges { get; set; }
        public double salesAmount { get; set; }
        public double saleAvgDiscount { get; set; }
        public int billingId { get; set; }
        public int shippingId { get; set; }
        public int orderStatus { get; set; }
        public int? orderMergedId { get; set; }
        public DateTime orderCreatedOn { get; set; }
        public int orderType { get; set; }
        public int? ShipmentMode { get; set; }
        public string ShippingCompany { get; set; }
        public string TrackingNumber { get; set; }
        public string remark { get; set; }
        public int loginDeviceId { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedOn { get; set; }
    }

    public class CustOrderDetailModel
    { 
      
        public int orderDetailsId { get; set; }
        public int inventoryId { get; set; }
        public int customerId { get; set; } 
    }
}
