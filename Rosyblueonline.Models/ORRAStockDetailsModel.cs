using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class ORRAStockDetailsModel
    {
        public string LotNumber { get; set; } 
        public string Shape { get; set; }
        public string Carat { get; set; }
        public string Color { get; set; }
        public string Clarity { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Depth { get; set; }
        public string HeartAndArrow { get; set; }
        public string Cut { get; set; }
        public string Polish { get; set; }
        public string Symmetry { get; set; }
        public string Fluorescence { get; set; }
        public string Lab { get; set; }
        public string RapnetPrice { get; set; }
        public string RapAmount { get; set; }
        public string Discount { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string CertificateNo { get; set; }
        public string DepthPerc { get; set; }
        public string TablePerc { get; set; }
        public string Girdle { get; set; }
        public string CrownHeight { get; set; }
        public string PavilionDepth { get; set; }
        public string PavilionAngle { get; set; }
        public string StarLength { get; set; }
        public string LowerHalf { get; set; }
        public string GirdlePerc { get; set; }
        public string Keytosymbol { get; set; }
        public string Reportdate { get; set; }
        public string EyeClean { get; set; }
        public string Shade { get; set; }
        public string TableBlack { get; set; }
        public string SideBlack { get; set; }
        public string Milky { get; set; }
        public string CuletSize { get; set; }
        public string OpensName { get; set; }
        public string GroupName { get; set; }
        public string MemberComments { get; set; }
        public string V360 { get; set; } 
        public string Stockstatus { get; set; }

        

    }

    public class PlaceOrderOrra
    {
        public int validCount { get; set; }
        public int InvalidCount { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }  
    }

    public class ORRAStockDetailsValidate
    {
        public List<ORRAStockDetailsModel> StockDetails { get; set; }
         public List<PlaceOrderOrra> OrderDetails { get; set; }

    }
}
