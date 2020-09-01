using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class orderItemDetailModel
    {
        [Key]
        public int orderItemDetailsId { get; set; }
        public int orderDetailsId { get; set; }
        public int inventoryId { get; set; }
        public double diaweight { get; set; }
        public double rapPrice { get; set; }
        public double rapDiscount { get; set; }
        public double rapPricect { get; set; }
        public double rapAmount { get; set; }
        public double? salesDiscount { get; set; }
        public double? salesPricect { get; set; }
        public double? salesAmount { get; set; }
    }
}
