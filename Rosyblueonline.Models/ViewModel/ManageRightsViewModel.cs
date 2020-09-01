using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Models.ViewModel
{
    public class ManageRightsViewModel
    {
        public List<SelectOptionsViewModel> Users { get; set; }
        public List<DownloadList> Menus { get; set; }
        public List<DownloadRightView> Data { get; set; }
    }
}
