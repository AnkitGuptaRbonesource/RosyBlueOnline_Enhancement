using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class DownloadRightModel
    {
        [Key, Column(Order = 0)]
        public int LoginID { get; set; }
        [Key, Column(Order = 1)]
        public int DownloadID { get; set; }
    }
}
