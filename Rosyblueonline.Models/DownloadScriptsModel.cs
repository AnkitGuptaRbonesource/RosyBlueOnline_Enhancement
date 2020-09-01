using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class DownloadScriptModel
    {
        [Key]
        public int downloadID { get; set; }
        public int loginID { get; set; }
        public string selectQuery { get; set; }
        public string joinScript  { get; set; }
        public string whereClause { get; set; }
        public string templateName { get; set; }
        public string templateDisplayName { get; set; }
        public string fieldString { get; set; }
        public string whereString { get; set; }
        public bool isActive { get; set; }
        public DateTime createdDate { get; set; }
    }
}
