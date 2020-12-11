using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class mstFAQBankModel
    {
         [Key]
        public int faqID { get; set; }
        public string Question { get; set; }
        public int? QuestionTypeId { get; set; }
        public int ParentFaqID { get; set; }
        public bool isActive { get; set; }
    }
}
