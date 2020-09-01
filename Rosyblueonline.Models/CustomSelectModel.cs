using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class CustomSelectModel
    {
        [Key]
        public int customSelectID { get; set; }
        public string fieldName { get; set; }
        public string selectField { get; set; }
        public string displayName { get; set; }
        public bool isJoin { get; set; }
        public string joinMasterTable { get; set; }
        public int SearchResultOrder { get; set; }
        public bool isActive { get; set; }
    }
}
