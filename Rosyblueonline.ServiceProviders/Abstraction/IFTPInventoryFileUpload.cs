using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
   public interface IFTPInventoryFileUpload
    {

        List<mstUploadFormatViewModel> InventoryUploadTypes(params string[] parameters);

    }
}
