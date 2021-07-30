using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{

    public class MarketInventoryUpload
    {
        public string Stock 
        { get; set; }
        public string CertificateNo
        { get; set; }
        public string Shape
        { get; set; }
        public string Weight
        { get; set; }
        public string Color
        { get; set; }
        public string Clarity
        { get; set; }
        public string Length
        { get; set; }
        public string Width
        { get; set; }
        public string Depth
        { get; set; }
        public string CutGrade
        { get; set; }
        public string Polish
        { get; set; }
        public string Symmetry
        { get; set; }
        public string Fluorescence
        { get; set; }
        public string Lab
        { get; set; }
        public string RapnetPrice
        { get; set; }
        public string Discount_per
        { get; set; }
        public string Pricect
        { get; set; }
        public string Amount
        { get; set; }
        public string Depth_per
        { get; set; }
        public string Table_per
        { get; set; }
        public string CrownHeight
        { get; set; }
        public string CrownAngle
        { get; set; }
        public string PavilionDepth
        { get; set; }
        public string PavilionAngle
        { get; set; }
        public string StarLength
        { get; set; }
        public string LowerHalf
        { get; set; }
        public string Girdle_per
        { get; set; }
        public string Keytosymbol
        { get; set; }
        public string Customer 
        { get; set; }



    }



    public class DiscountPriceListUpload
    {
        public string Cut
        { get; set; }
        public string Polish
        { get; set; }
        public string Symmetry
        { get; set; }
        public string SizeFrom
        { get; set; }
        public string SizeTo
        { get; set; }
        public string Color
        { get; set; }
        public string Clarity
        { get; set; }
        public string Discount
        { get; set; }
    }

    public class RBDiscountMasterUpload
    {
        public string SHAPE
        { get; set; }
        public string CUT
        { get; set; }
        public string POL
        { get; set; }
        public string SYMM
        { get; set; }
        public string SIZEFROM
        { get; set; }
        public string SIZETO
        { get; set; }
        public string COLOR
        { get; set; }
        public string CLARITY
        { get; set; }
        public string DISCOUNT
        { get; set; }
    }

    public class CustomerDiscountMasterUpload
    {
        public string SHAPE
        { get; set; }
        public string CUT
        { get; set; }
        public string POL
        { get; set; }
        public string SYMM
        { get; set; }
        public string SIZEFROM
        { get; set; }
        public string SIZETO
        { get; set; }
        public string COLOR
        { get; set; }
        public string CLARITY
        { get; set; }
        public string DISCOUNT
        { get; set; }
    }

    public class FloraDiscountMasterUpload
    { 
        public string CUT
        { get; set; }
        public string POL
        { get; set; }
        public string SYMM
        { get; set; }
        public string FLORA
        { get; set; } 
        public string COLOR
        { get; set; }
        public string CLARITY
        { get; set; }
        public string DISCOUNT
        { get; set; }
    }

}
