using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rosyblueonline.Models
{ 

    public class MarketingStockSummaryModel
    {
        public List<SaleSViewModel> SaleS { get; set; }
        public List<SYearViewModel> SYear { get; set; }
        public List<SMonthViewModel> SMonth { get; set; }
        public List<SLocationViewModel> SLocation { get; set; }
          

    }
    public class SaleSViewModel
    { 
        public int  ID { get; set; }
        public string  Name { get; set; } 
    }
    public class SYearViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class SMonthViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class SLocationViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class MarketingStockSummaryDetailsModel
    {
        public string companyName { get; set; }

        public string sellMonth { get; set; }

        public int NoOfStone { get; set; }
        public decimal TotalWeight { get; set; }

        public decimal TotalValue { get; set; }
        public string LotNoList { get; set; }

         

    }

}
