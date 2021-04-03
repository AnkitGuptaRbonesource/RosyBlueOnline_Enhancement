using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class DownloadList
    {
        [Key]
        public int RowID { get; set; }
        public string Name { get; set; }
        public string BindedDownloadID { get; set; }
        public string SheetNames { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
