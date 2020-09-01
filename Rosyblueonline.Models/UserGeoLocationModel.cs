using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class UserGeoLocationModel
    {
        [Key]
        public int LocationID { get; set; }
        public int LoginID { get; set; }
        public string Username { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public class UserMenuAccessModel
    {  
        public int? Home { get; set; }
        public int? Dashboard { get; set; }
        public int? MyAccount { get; set; }
        public int? AllOrder { get; set; }
        public int? AllMenu { get; set; }
        public int? Search { get; set; }
        public int? Inventory { get; set; }
        public int? StockSummary { get; set; }
        public int? InventoryUpload { get; set; }
        public int? InventoryDownload { get; set; }
        public int? UploadHistory { get; set; }
        public int? StoneStatus { get; set; }
        public int? StoneHistory { get; set; }
        public int? GIAReports { get; set; }
        public int? Marketing { get; set; }
        public int? BlueNile { get; set; }
        public int? JamesAllen { get; set; }
        public int? JamesAllenHK { get; set; }
        public int? Utility { get; set; }
        public int? SummaryDiscount { get; set; }
        public int? BlockSite { get; set; }














    }
}
