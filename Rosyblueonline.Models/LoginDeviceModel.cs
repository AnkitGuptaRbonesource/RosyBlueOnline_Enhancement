using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class LoginDeviceModel
    {
        [Key]
        public int loginDeviceId { get; set; }
        public string deviceName { get; set; }
        public int? loginID { get; set; }
        public string ipAddress { get; set; }
        public DateTime createdOn { get; set; }
    }
}
