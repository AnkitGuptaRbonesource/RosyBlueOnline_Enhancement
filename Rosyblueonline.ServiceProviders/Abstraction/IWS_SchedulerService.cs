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

    public interface IWS_SchedulerService
    {
       // List<WS_SchedulerModel> WS_SchedulerList();
        IQueryable<WS_SchedulerModel> SchedulerList();

        int UpdateScheduler(int WSID, string Name,string Frequency, int FrequencyInt, bool Status);
    }
    
}
