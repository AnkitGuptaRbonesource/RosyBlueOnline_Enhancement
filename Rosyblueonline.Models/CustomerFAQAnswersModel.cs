using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class CustomerFAQAnswersModel
    {

        [Key]
        public int Id { get; set; }
        public int FAQId { get; set; }
        public int QuestionTypeId { get; set; }
        public string FAQOptionsAnswer { get; set; }
        public string FAQTextAnswer { get; set; } 
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; } 
    }
}
