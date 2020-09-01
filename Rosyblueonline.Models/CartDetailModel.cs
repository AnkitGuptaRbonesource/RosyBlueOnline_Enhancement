using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class CartDetailModel
    {
        [Key]
        public int cartDetailsId { get; set; }
        public int loginID { get; set; }
        public int inventoryID { get; set; }
        public double salePrice { get; set; }
        public double saleDiscount { get; set; }
        public DateTime createdOn { get; set; }
        public string tokenId { get; set; }
        public bool isBlocked { get; set; }
    }
}
