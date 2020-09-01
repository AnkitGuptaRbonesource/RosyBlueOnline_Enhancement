using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class inventoryModel
    {
        [Key]
        public int inventoryID { get; set; }
        public string lotNumber { get; set; }
        public string certificateNo { get; set; }
        public string rfid { get; set; }
        public int shapeID { get; set; }
        public double diaWeight { get; set; }
        public int colorID { get; set; }
        public bool? isFancy { get; set; }
        public int? fancyColor { get; set; }
        public int? fancyIntensity { get; set; }
        public int? fancyOvertone { get; set; }
        public string fancyDescription { get; set; }
        public int clarityID { get; set; }
        public double diaLength { get; set; }
        public double diaWidth { get; set; }
        public double diaDepth { get; set; }
        public int cutGradeID { get; set; }
        public int labID { get; set; }
        public double pricePerCT_upload { get; set; }
        public double discPercentage_upload { get; set; }
        public double finalAmount_upload { get; set; }
        public double discount { get; set; }
        public double rapPrice { get; set; }
        public double pricePerCT { get; set; }
        public double finalAmount { get; set; }
        public double depthPercent { get; set; }
        public double tablePercent { get; set; }
        public int girdleID { get; set; }
        public int? girdleNatureID { get; set; }
        public int polishID { get; set; }
        public int symmetryID { get; set; }
        public int fluorescenceID { get; set; }
        public double crownHeight { get; set; }
        public double crownAngle { get; set; }
        public double pavilionDepth { get; set; }
        public double pavilionAngle { get; set; }
        public double starLength { get; set; }
        public double lowerHalf { get; set; }
        public string foreverMark { get; set; }
        public double girdlePercentage { get; set; }
        public int? brilliance { get; set; }
        public string fire { get; set; }
        public string sparkle { get; set; }
        public string laserinscribe { get; set; }
        public string gemxid { get; set; }
        public int? shadeId { get; set; }
        public int? tableBlackInclusion { get; set; }
        public int? sideBlackInclusion { get; set; }
        public int? milkyInclusion { get; set; }
        public int? groupId { get; set; }
        public DateTime? reportdate { get; set; }
        public int? culetSize { get; set; }
        public string userComments { get; set; }
        public int? eyeCleanID { get; set; }
        public int? customerId { get; set; }
        public int? heartsandArrows { get; set; }
        public int? opensId { get; set; }
        public int? salesLocationID { get; set; }
        public string keytosymbol { get; set; }
        public string giaComments { get; set; }
        public string v360url { get; set; }
        public string Rough { get; set; }
        public string jaUrl { get; set; }
        public string DiacamUrl { get; set; }
        public int? gMeterid { get; set; }
        public int? gmeterValue { get; set; }
        public int stockStatus { get; set; }
        public bool? isConfirmedMemo { get; set; }
        public int? culetNatureID { get; set; }
        public int? fileId { get; set; }
        public int? orderDetailsId { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime? lastUpdateOn { get; set; }
        //public int createdby { get; set; }
        public string lastUpdateIP { get; set; }
        //public bool? isRevised { get; set; }
        public DateTime? revisedDate { get; set; }
        public DateTime? soldDate { get; set; }

    }
}
