using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MstStateModel
    {
        [Key]
        public int stateId { get; set; }
        public string stateName { get; set; }
        public int countryId { get; set; }
        public bool isActive { get; set; }
    }
}
