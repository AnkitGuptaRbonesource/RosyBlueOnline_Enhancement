using Rosyblueonline.Framework;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Implementation
{
 
    public class WS_SchedulerService : IWS_SchedulerService
    { 
        readonly UnitOfWork uow = null;
        DataContext context = null;
        public WS_SchedulerService(IUnitOfWork uow, IDataContext context)
        {
            this.uow = uow as UnitOfWork;
            this.context = context as DataContext;
        }

       
        //public List<WS_SchedulerModel> WS_SchedulerList()
        //{
        //    return uow.WS_SchedulerM.GetAll().ToList(); 
        //}

        public IQueryable<WS_SchedulerModel> SchedulerList()
        {
            return this.context.WS_Scheduler.AsQueryable();
        }
        public int UpdateScheduler(int WSID,string Name, string Frequency, int FrequencyInt, bool Status)
        {

            WS_SchedulerModel obj = this.uow.WS_SchedulerM.Queryable().Where(x => x.WSID == WSID).FirstOrDefault();

            obj.Name = Name;
            obj.Frequency = Frequency;
            obj.FrequencyInterval = FrequencyInt;
            obj.Status = Status;
            this.uow.WS_SchedulerM.Edit(obj);
            return this.uow.Save();
             
        }

    }
}
