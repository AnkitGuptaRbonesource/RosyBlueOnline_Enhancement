using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models
{
    public class MenuMasterModel
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int MainMenuId { get; set; }
        public bool IsActive { get; set; } 
    }
}
