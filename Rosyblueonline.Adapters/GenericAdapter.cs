using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Adapters
{
    public class GenericAdapter
    {
        public virtual M Convert<E, M>(E entity) where E : class where M : class
        {
            var model = Mapper.Map<E, M>(entity);
            return model;
        }
    }
}
