using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Rosyblueonline.Models;
namespace Rosyblueonline.Adapters
{
    public class BillingAddressAdapter : IAdapter<MstBillingAddressModel, mstBillingAddress>
    {
        public MstBillingAddressModel ConvertEntityToModel(mstBillingAddress entity)
        {
            var model = Mapper.Map<mstBillingAddress, MstBillingAddressModel>(entity);
            return model;
        }

        public mstBillingAddress ConvertModelToEntity(MstBillingAddressModel model)
        {
            var entity = Mapper.Map<MstBillingAddressModel, mstBillingAddress>(model);
            return entity;
        }

        public IEnumerable<MstBillingAddressModel> ConvertEntitiesToModels(IEnumerable<mstBillingAddress> IEntity)
        {
            var models = Mapper.Map<IEnumerable<mstBillingAddress>, IEnumerable<MstBillingAddressModel>>(IEntity);
            return models;
        }
    }
}
