using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Abstraction
{
    public interface IDownloadScriptService
    {
        List<DownloadList> GetForDisplay();
        IQueryable<DownloadRightView> GetDownloadRightView();
        List<DownloadRightModel> GetByLoginID(int LoginID);
        int UpdateRights(List<DownloadRightModel> objLst);
        List<DownloadList> GetDownloadForMenu(int LoginID, int RoleID);
        DataSet ExecuteDownload(string Ids);

        List<DownloadList> GetMarketDownloadForMenu(int LoginID);

        DataTable MarketInventoryDownloadExcelExport(string LoginID, string QFlag);

    }
}
