using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MstSalesLocationModel
    {
        [Key]
        public int salesLocationID { get; set; }
        public string locationName { get; set; }
        public int? countryID { get; set; }
        public double? ftShippingCost { get; set; }
        public DateTime createdOn { get; set; }
        public bool isActive { get; set; }
    }
}
