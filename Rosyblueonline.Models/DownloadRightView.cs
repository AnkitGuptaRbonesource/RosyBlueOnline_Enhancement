using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class DownloadRightView
    {
        [Key]
        public int loginID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int roleID { get; set; }
        public string DownloadNames { get; set; }
    }
}
