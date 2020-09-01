using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class RFIDService : IRFIDService
    {
        UnitOfWork uow = null;
        public RFIDService(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        public int ReleaseRFID(string[] RFIDs)
        {
            List<inventoryModel> objLstIvn = this.uow.Inventory.Queryable().Where(x => RFIDs.Contains(x.rfid)).ToList();
            for (int i = 0; i < objLstIvn.Count; i++)
            {
                objLstIvn[i].rfid = "";
                this.uow.Inventory.Edit(objLstIvn[i]);
            }
            return uow.Save();
        }

        //InventoryUploadRFID(DataTable dt, int LoginID, string Ipaddress, DateTime dateofUpdate, int fileId)
        public List<RFIDTempTableMiewModel> InventoryUploadRFID(DataTable dt, int Login, string IpAddress, int FileID)
        {
            return this.uow.Inventory.InventoryUploadRFID(dt, Login, IpAddress, DateTime.Now, FileID);
        }

        public int InventoryUpdateRFIDStatus(string rfid, bool Status)
        {
            mstRFIDModel objRFID = this.uow.MstRFID.GetAll(x => x.RfidNo == rfid).FirstOrDefault();
            if (objRFID != null)
            {
                objRFID.Isactive = Status;
                this.uow.MstRFID.Edit(objRFID);
                return this.uow.Save();
            }
            return 0;
        }

        public List<RFIDstockmaster> RFIDstockTally(string stockId, string RFId, string boxName, string Rfidmachine, int loginid, string Boxid, string RaiseEvent)
        {
            return this.uow.Inventory.RFIDstockTally(stockId, RFId, boxName, Rfidmachine, loginid, Boxid, RaiseEvent);
        }

        public List<RFIDexportBox> RFIdstockBoxExport(string stockId, string RFId, string boxName, string Rfidmachine, int loginid, string Boxid, string RaiseEvent)
        {
            return this.uow.Inventory.RFIdstockBoxExport(stockId, RFId, boxName, Rfidmachine, loginid, Boxid, RaiseEvent);
        }

        public List<mstRFIDModel> RFIDmaster()
        {
            return this.uow.MstRFID.GetAll().ToList();
        }

        public int AddRFID(DataTable dt, int LoginID)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int currentStatus = Convert.ToInt32(dt.Rows[i][1]);
                string Rfid = dt.Rows[i][0].ToString();
                if (!this.uow.MstRFID.Queryable().Where(x => x.RfidNo == Rfid).Any())
                {
                    this.uow.MstRFID.Add(new mstRFIDModel
                    {
                        RfidNo = Rfid,
                        CurrentStatus = currentStatus,
                        Createdby = LoginID,
                        Isactive = true,
                        CreatedOn = DateTime.Now
                    });
                }
            }
            return this.uow.Save();
        }

        public IQueryable<RFIDhistoryViewModel> RFIDMasterQueryable()
        {
            return (from rf in this.uow.RFIDhistory.Queryable()
                    join b in this.uow.MstBillingAddresses.Queryable() on rf.Createdyby equals b.loginID
                    select new RFIDhistoryViewModel
                    {
                        Rowno = rf.Rowno,
                        RFIDno = rf.RFIDno,
                        CertificateNO = rf.CertificateNO,
                        activityId = rf.activityId,
                        CreatedOn = rf.CreatedOn,
                        Createdyby = rf.Createdyby,
                        CreatedByName = b.firstName + " " + b.lastName
                    });
        }

        public TallyStockViewModel TallyStockByRFID(string RFIDs, int CustomerID)
        {
            TallyStockViewModel obj = new TallyStockViewModel();
            obj.StockCount = this.uow.ExecuteQuery<TallyStockByRFIDViewModel>("Exec proc_TallyStockByRfid '{0}'", RFIDs);
            obj.Inventory = this.uow.ExecuteQuery<inventoryDetailsViewModel>("Exec proc_CustomSiteSearch '{0}','{1}',0,500000,'LotNumber','asc','RFIDPhysicalStock','SpecialSearch'", CustomerID.ToString(), "RFID~" + RFIDs);
            return obj;
        }

        public MemoTallyStockByRfidViewModel MemoTallyStockByRfid(int LoginID, int OrderID, string RFIDs)
        {
            MemoTallyStockByRfidViewModel objModel = uow.Orders.MemoTallyStockByRfid(LoginID, OrderID, RFIDs);
            objModel.Final = new List<inventoryDetailsViewModel>();
            for (int i = 0; i < objModel.InMemo.Count; i++)
            {
                if (string.IsNullOrEmpty(objModel.InMemo[i].rfid))
                {
                    objModel.InMemo[i].RFIDStatus = "RFID Not Assigned";
                    objModel.Final.Add(objModel.InMemo[i]);
                    objModel.InMemo.RemoveAt(i);
                    i--;
                    continue;
                }
                for (int j = 0; j < objModel.InBox.Count; j++)
                {
                    if (objModel.InMemo[i].inventoryID == objModel.InBox[j].inventoryID)
                    {
                        objModel.InMemo[i].RFIDStatus = "On Memo";
                        objModel.Final.Add(objModel.InMemo[i]);
                        objModel.InMemo.RemoveAt(i);
                        objModel.InBox.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }

            for (int i = 0; i < objModel.InMemo.Count; i++)
            {
                objModel.InMemo[i].RFIDStatus = "Missed Lotnumber";
                objModel.Final.Add(objModel.InMemo[i]);
            }

            for (int i = 0; i < objModel.InBox.Count; i++)
            {
                objModel.InBox[i].RFIDStatus = "Not This Memo";
                objModel.Final.Add(objModel.InBox[i]);
            }
            return objModel;
        }
    }
}
