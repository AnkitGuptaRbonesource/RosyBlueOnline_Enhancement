using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Framework
{
    //public interface IResponse<T> where T : class
    //{
    //    int Code { get; set; }
    //    string Message { get; set; }
    //    T Result { get; set; }
    //    bool IsSuccess { get; set; }
    //}
    public class Response //, IResponse<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public dynamic Result { get; set; }

        public bool IsSuccess { get; set; }
    }
}
