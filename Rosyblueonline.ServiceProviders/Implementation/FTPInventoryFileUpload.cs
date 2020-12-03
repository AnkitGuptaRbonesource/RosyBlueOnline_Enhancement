using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosyblueonline.ServiceProviders.Abstraction;
using Rosyblueonline.Adapters;
using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using System.Web.Mvc;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.UnitOfWork;
using System.Data;
using Rosyblueonline.Repository.Context;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class FTPInventoryFileUpload: IFTPInventoryFileUpload
    {

        readonly UnitOfWork uow = null;
        readonly DBSQLServer dBSQL = null;
        MailUtility objMU = null;
         
        public FTPInventoryFileUpload(IUnitOfWork uow, IDBSQLServer dBSQL)
        {
            this.uow = uow as UnitOfWork;
            this.dBSQL = dBSQL as DBSQLServer;
        }
 

        public List<mstUploadFormatViewModel> InventoryUploadTypes(params string[] parameters)
        {
           
            return this.uow.Inventory.InventoryUploadTypes(parameters);
        }

    }
}
