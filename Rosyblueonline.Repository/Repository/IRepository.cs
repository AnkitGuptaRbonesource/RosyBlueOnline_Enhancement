using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        //IEnumerable<T> ExecWithStoreProcedureWithParamEntity(string spQuery, params object[] parameters);
        //IEnumerable<T> ExecStoreProcedureNoParamEntity(string query);
        //T ExecWithStoreProcedureWithParamEntitySingle(string spQuery, object parameters);
        //T ExecWithStoreProcedureWithParamEntityMulti(string spQuery, params object[] parameters);
        //T ExecStoreProcedureNoParamEntitySingle(string query);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);
    }
}
