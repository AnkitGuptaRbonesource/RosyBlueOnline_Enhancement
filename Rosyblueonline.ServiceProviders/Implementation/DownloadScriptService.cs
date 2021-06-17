using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
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
    public class DownloadScriptService : IDownloadScriptService
    {
        UnitOfWork uow = null;
        DBSQLServer db = null;
        public DownloadScriptService(IUnitOfWork uow, IDBSQLServer db)
        {
            this.uow = uow as UnitOfWork;
            this.db = db as DBSQLServer;
        }

        public List<DownloadList> GetForDisplay()
        {
            return this.uow.DownloadList.Queryable().ToList();
        }

        public IQueryable<DownloadRightView> GetDownloadRightView()
        {
            return this.uow.DownloadRightView.Queryable();
        }

        public List<DownloadList> GetDownloadForMenu(int LoginID, int RoleID)
        {
            //RoleID 
            //1   SUPERADMIN
            //2   ADMIN
            if (RoleID != 1 && RoleID != 2)
                return (from dl in uow.DownloadList.Queryable()
                        join dr in uow.DownloadRights.Queryable() on dl.RowID equals dr.DownloadID
                        where dr.LoginID == LoginID && dl.IsActive == true
                        select dl).OrderBy(n => n.DisplayOrder).ToList();
            else
                return uow.DownloadList.Queryable().Where(x => x.IsActive == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public List<DownloadRightModel> GetByLoginID(int LoginID)
        {
            return this.uow.DownloadRights.Queryable().Where(x => x.LoginID == LoginID).ToList();
        }

        public int UpdateRights(List<DownloadRightModel> objLst)
        {
            List<DownloadRightModel> objOldRights = new List<DownloadRightModel>();
            if (objLst.Count > 0)
            {
                objOldRights = GetByLoginID(objLst[0].LoginID);
                for (int i = 0; i < objOldRights.Count; i++)
                {
                    this.uow.DownloadRights.Delete(objOldRights[i]);
                }
                for (int i = 0; i < objLst.Count; i++)
                {
                    this.uow.DownloadRights.Add(objLst[i]);
                }
                return this.uow.Save();
            }
            return 0;
        }

        public DataSet ExecuteDownload(string Ids)
        {
            DataSet ds = new DataSet();
            string[] DownloadID = Ids.Split(',');
            for (int i = 0; i < DownloadID.Length; i++)
            {
                DataSet dsResult = this.db.ExecuteCommand("exec proc_DownloadResult " + DownloadID[i], CommandType.Text);
                DataTable dt = dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                if (dt != null && dt.Columns[0].ColumnName == "RowNum")
                {
                    dt.Columns.RemoveAt(0);

                }
                DataTable dtCopy = dt.Copy();
                ds.Tables.Add(dtCopy);
            }
            return ds;
        }



        public List<DownloadList> GetMarketDownloadForMenu(int LoginID)
        {
            List<DownloadList> objInvVM = new List<DownloadList>();

            return objInvVM = MarketInvDownload<DownloadList>(LoginID.ToString(), "MarketList");


        }


        public DataTable MarketInventoryDownloadExcelExport(string LoginID, string QFlag)
        {
            // List<InventoryDownloadViewModel> objInvVM = new List<InventoryDownloadViewModel>();

            //   objInvVM = MarketInvDownload<InventoryDownloadViewModel>(LoginID, QFlag);
            //  DataTable dt = Rosyblueonline.Framework.ListtoDataTable.ToDataTable<InventoryDownloadViewModel>(objInvVM);


            DataSet dsResult = this.db.ExecuteCommand("exec proc_MarketInventoryDownload " + LoginID + "," + QFlag, CommandType.Text);


            return dsResult.Tables[0];
        }

        public List<T> MarketInvDownload<T>(params string[] Parameters) where T : class
        {

            return this.uow.ExecuteQuery<T>("Exec proc_MarketInventoryDownload '{0}','{1}'", Parameters);


        }

    }
}
