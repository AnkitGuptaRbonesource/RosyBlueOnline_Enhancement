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
        public int? AllMemo { get; set; }
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
        public int? RFID { get; set; }
        public int? RFIDSearch { get; set; }
        public int? RFIModify { get; set; }
        public int? RFIDUploadHistory { get; set; }
        public int? RFIDStockTally { get; set; }
        public int? RFIDMaster { get; set; }
        public int? RFIDHistory { get; set; }
        public int? Profile { get; set; }
        public int? Changepassword { get; set; }
        public int? AddCustomer { get; set; }
        public int? AddRoleRights { get; set; }

        public int? SellSummary { get; set; } 
        public int? MarketingInventory { get; set; }
        public int? MarketInventoryUpload { get; set; }
        public int? MarketInventoryDownload { get; set; }


    }
}
