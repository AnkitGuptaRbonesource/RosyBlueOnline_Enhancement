using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        int Save();

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        List<T> ExecuteQuery<T>(string Text, params string[] Parameters) where T : class;
    }
}
