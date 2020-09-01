using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class StockHistoryViewModel
    {
        [Key]
        public int StoneHistoryid { get; set; }
        public string Lotnumber { get; set; }
        public int? Loginid { get; set; }
        public int? CustomerID { get; set; }
        public int ActivityId { get; set; }
        public string Remark { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string CompanyName { get; set; }
        public string CustFirstName { get; set; }
        public string CustLastName { get; set; }
        public string shapeName { get; set; }
        public double diaWeight { get; set; }
        public string clarityName { get; set; }
        public string colorName { get; set; }
        public string cutGradeName { get; set; }
        public double pricePerCT  { get; set; }
        public double rapPrice    { get; set; }
        public double discount    { get; set; }
        public double newDiscount { get; set; }
        public double newRapPrice { get; set; }
        public string FlagDescription { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
