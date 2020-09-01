using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class BlockSiteHistoryModel
    {
        [Key]
        public int Blockid { get; set; }
        public int Loginid { get; set; }
        public int Roleid { get; set; }
        public bool Isblocked { get; set; }
        public DateTime Createdon { get; set; }
        public string TokenID { get; set; }
    }
}
