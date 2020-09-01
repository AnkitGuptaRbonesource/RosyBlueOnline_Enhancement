using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Rosyblueonline.Models
{
   public class fileUploadLogModel
    {
        [Key]
        public int fileId { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime completedOn { get; set; }
        public string ipAddress { get; set; }
        public int uploadType { get; set; }
        public string refData { get; set; }
        public int uploadStatus { get; set; }
        public int validInv { get; set; }
        public int invalidInv { get; set; }
    }
}
