using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rosyblueonline.Models
{
    public class MarketFileUploadLogModel
    {
        [Key]
        public int fileId { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime completedOn { get; set; }
        public string ipAddress { get; set; }
        public string uploadStatus { get; set; }
        public int TotalInv { get; set; }
        public int validInv { get; set; }
        public int InvalidInv { get; set; }
        public int QCDone { get; set; }
        public int QCPending { get; set; }
        public bool IsActive { get; set; }

    }

    public class VendorListModel
    {
        [Key]
        public string customerId { get; set; }
        public string customerName { get; set; }

    }



    public class QCFileUploadLogModel
    {
        [Key]
        public int fileId { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime completedOn { get; set; }
        public string ipAddress { get; set; }
        public string uploadStatus { get; set; }
        public int validInv { get; set; }
        public bool IsActive { get; set; }
        public string DisUpdateStatus { get; set; }


    }


    public class QCFinalDetailsModel
    {
        [Key]
        public int inventoryID { get; set; }
        public string certificateNo { get; set; }
        public string lotNumber { get; set; }
        public string tableblackinclusion { get; set; }
        public string sideBlackInclusion { get; set; }
        public string opensId { get; set; }
        public string milkyInclusion { get; set; }
        public string shadeId { get; set; }
        public string Remark { get; set; }

        public string Status { get; set; }
        public DateTime createdOn { get; set; }
        public string fileId { get; set; }


        public string size { get; set; }
        public string color { get; set; }
        public string clarity { get; set; }
        public string Criteria { get; set; }




    }


    public class QCFinalDDLListModel
    {
        public List<DDlList> Fileids { get; set; }

        public List<DDlList> TableBlack { get; set; }
        public List<DDlList> SideBlack { get; set; }
        public List<DDlList> OpensName { get; set; }
        public List<DDlList> Milky { get; set; }
        public List<DDlList> Shade { get; set; }
        public List<VendorListModel> VendorList { get; set; }


        
    }


    public class DDlList
    {
        public string id { get; set; }
        public string Name { get; set; }
    }




    public class DiscountMasterFileUploadLogModel
    {
        [Key]
        public int fileId { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime completedOn { get; set; }
        public string ipAddress { get; set; }
        public string uploadStatus { get; set; }
        public int validInv { get; set; }
        public bool IsActive { get; set; }
        public string UploadId { get; set; }


    }



    public class QCFinalFileIdsListModel
    {
        public List<DDLFileids> Fileids { get; set; }

        public List<DDLFileids> Fileids2 { get; set; } 



    }

    public class DDLFileids
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
