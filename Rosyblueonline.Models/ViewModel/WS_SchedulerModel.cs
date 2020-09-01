using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
   public class WS_SchedulerModel
    {
        [Key]
        public int WSID { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public int FrequencyInterval { get; set; }
        public bool Status { get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextRun { get; set; }
        public int modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
    }
}
