using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class UserActivityLogModel
    {

        [Key]
        public int ActivityId { get; set; }
        public int LoginId { get; set; }
        public string ActionName { get; set; }
        public string ActionDetails { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
