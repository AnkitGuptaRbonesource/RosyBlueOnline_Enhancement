using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Adapters
{
    public interface IAdapter<TModel, TEntity>
       where TModel : class
       where TEntity : class
    {
        TModel ConvertEntityToModel(TEntity entity);
        TEntity ConvertModelToEntity(TModel model);
        IEnumerable<TModel> ConvertEntitiesToModels(IEnumerable<TEntity> lEntity);
    }
}
