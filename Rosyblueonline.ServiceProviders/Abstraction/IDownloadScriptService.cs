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

        DataTable MarketInventoryDownloadExcelExport(string LoginID, string FileId, string QFlag, string UploadDate, string VendorName, string CertNos);

        IQueryable<MarketFileUploadLogModel> QueryableFilesDetail();

        IQueryable<QCFileUploadLogModel> QueryableQCFilesDetail();

        IQueryable<DiscountMasterFileUploadLogModel> QueryableDiscountFilesDetail();

        DataTable MInventory_QCPriceUpdate(string FileId, string LoginID);

    }
}
