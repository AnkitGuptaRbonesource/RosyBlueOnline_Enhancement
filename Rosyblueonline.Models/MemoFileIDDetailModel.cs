using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MemoFileIDDetailModel
    {
        [Key]
        public int memoDetailsId { get; set; }
        public int orderDetailsId { get; set; }
        public int? fileID { get; set; }
        public DateTime createdOn { get; set; }
    }
}
