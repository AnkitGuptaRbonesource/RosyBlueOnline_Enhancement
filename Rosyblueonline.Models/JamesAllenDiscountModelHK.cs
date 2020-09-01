using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class JamesAllenDiscountHKModel
    {
        [Key]
        public int SrNo { get; set; }
        public decimal caratRange1Disc { get; set; }
        public decimal caratRange2Disc { get; set; }
        public decimal caratRange3Disc { get; set; }
        public decimal caratRange4Disc { get; set; }
        public decimal caratRange5Disc { get; set; }
        public decimal caratRange6Disc { get; set; }
        public decimal caratRange7Disc { get; set; }
        public decimal caratRange8Disc { get; set; }
        public decimal caratRange9Disc { get; set; }
        public decimal caratRange10Disc { get; set; }
        public decimal caratRange11Disc { get; set; }
        public decimal caratRange12Disc { get; set; }
        public decimal caratRange13Disc { get; set; }
        public decimal caratRange14Disc { get; set; }
        public decimal caratRange15Disc { get; set; }
        public decimal caratRange16Disc { get; set; }
        public decimal caratRange17Disc { get; set; }
        public decimal caratRange18Disc { get; set; }
        public decimal caratRange19Disc { get; set; }
        public decimal caratRange20Disc { get; set; }
        public decimal caratRange21Disc { get; set; }
        public decimal caratRange22Disc { get; set; }
        public decimal caratRange23Disc { get; set; }
        public decimal caratRange24Disc { get; set; }
        public decimal caratRange25Disc { get; set; }

        public decimal CNRDisc { get; set; }
        public decimal haExDisc { get; set; }
        public decimal haVgDisc { get; set; }
        public decimal cnrDiscHA { get; set; }

        public DateTime createdOn { get; set; }
        public int createdBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Isactive { get; set; }
    }
}
