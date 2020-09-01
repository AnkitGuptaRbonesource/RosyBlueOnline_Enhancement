using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Rosyblueonline.Entities;
using Rosyblueonline.Models;

namespace Rosyblueonline.Adapters
{
   public class ShippingAddressAdapter : IAdapter<MstShippingAddressModel, mstShippingAddress>
    {
        public MstShippingAddressModel ConvertEntityToModel(mstShippingAddress entity)
        {
            var model = Mapper.Map<mstShippingAddress, MstShippingAddressModel>(entity);
            return model;
        }

        public mstShippingAddress ConvertModelToEntity(MstShippingAddressModel model)
        {
            var entity = Mapper.Map<MstShippingAddressModel, mstShippingAddress>(model);
            return entity;
        }

        public IEnumerable<MstShippingAddressModel> ConvertEntitiesToModels(IEnumerable<mstShippingAddress> IEntity)
        {
            var models = Mapper.Map<IEnumerable<mstShippingAddress>, IEnumerable<MstShippingAddressModel>>(IEntity);
            return models;
        }
    }
}
