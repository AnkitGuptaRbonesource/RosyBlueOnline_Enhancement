using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MemoDetailModel
    {
        [Key]
        public int memoDetailsId { get; set; }
        public int orderDetailsId { get; set; }
        public DateTime createdOn { get; set; }
    }
}
