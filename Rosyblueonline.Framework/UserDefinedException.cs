using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Framework
{
    public class UserDefinedException : Exception
    {
        public UserDefinedException()
        {

        }

        public UserDefinedException(string name)
            : base(name, new Exception(StringResource.CustomError))
        {

        }
    }
}
