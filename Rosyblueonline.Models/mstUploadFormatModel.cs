using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class mstUploadFormatModel
    {
        [Key]
        public int uploadFormatId { get; set; }
        public string uploadFullName { get; set; }
        public string uploadValue { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFormatPath { get; set; }
        public DateTime? dtCreatedOn { get; set; }
        public int? uploadStatus { get; set; }
        public int? uploadOrder { get; set; }
        public string Description { get; set; }
    }
}
