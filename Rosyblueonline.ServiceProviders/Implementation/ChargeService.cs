using Rosyblueonline.Models;
using Rosyblueonline.Models.ViewModel;
using Rosyblueonline.Repository.UnitOfWork;
using Rosyblueonline.ServiceProviders.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.ServiceProviders.Implementation
{
    public class ChargeService : IChargeService
    {
        readonly UnitOfWork uow = null;
        public ChargeService(IUnitOfWork uow)
        {
            this.uow = uow as UnitOfWork;
        }

        public List<mstChargesModel> GetAllCharges()
        {
            return uow.MstChargesModel.GetAll().ToList();
        }

        public int Update(List<KeyValueViewModel> obj)
        {
            List<mstChargesModel> objLst = GetAllCharges();
            for (int i = 0; i < objLst.Count; i++)
            {
                KeyValueViewModel objKV = obj.Where(x => x.Key == objLst[i].InputName).FirstOrDefault();
                if (objKV != null)
                {
                    objLst[i].chargesValue = Convert.ToDecimal(objKV.Value);
                    uow.MstChargesModel.Edit(objLst[i]);
                }
            }
            return uow.Save();
        }
    }
}
