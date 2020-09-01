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
    public interface IRFIDService
    {
        int ReleaseRFID(string[] RFIDs);
        List<RFIDTempTableMiewModel> InventoryUploadRFID(DataTable dt, int Login, string IpAddress, int FileID);
        int InventoryUpdateRFIDStatus(string rfid, bool Status);
        List<RFIDstockmaster> RFIDstockTally(string stockId, string RFId, string boxName, string Rfidmachine, int loginid, string Boxid, string RaiseEvent);
        List<RFIDexportBox> RFIdstockBoxExport(string stockId, string RFId, string boxName, string Rfidmachine, int loginid, string Boxid, string RaiseEvent);
        List<mstRFIDModel> RFIDmaster();
        int AddRFID(DataTable dt, int LoginID);
        IQueryable<RFIDhistoryViewModel> RFIDMasterQueryable();
        TallyStockViewModel TallyStockByRFID(string RFIDs, int CustomerID);
        MemoTallyStockByRfidViewModel MemoTallyStockByRfid(int LoginID, int OrderID, string RFIDs);

    }
}
