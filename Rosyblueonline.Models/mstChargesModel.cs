using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class mstChargesModel
    {
        [Key]
        public int chargesId { get; set; }
        public string chargesName { get; set; }
        public string chargesAbbrivation { get; set; }
        public string InputName { get; set; }
        public int chargesType { get; set; }
        public decimal chargesValue    { get; set; }
        public string actionMode { get; set; }
        public bool isCappingApplied { get; set; }
        public decimal? minOrderValue { get; set; }
        public decimal? maxOrderValue { get; set; }
        public bool isActive { get; set; }
    }
}
