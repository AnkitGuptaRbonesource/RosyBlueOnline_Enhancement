using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Rosyblueonline.Repository.Context;

namespace Rosyblueonline.Repository
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private bool disposed = false;

        DataContext context = null;

        public Repository(IDataContext context)
        {
            this.context = context as DataContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        //public IEnumerable<T> ExecWithStoreProcedureWithParamEntity(string spQuery, params object[] parameters)
        //{
        //    try
        //    {
        //        var result = context.Database.SqlQuery<T>(spQuery, parameters).ToList();
        //        return result.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Source = "Repository->ExecWithStoreProcedureWithParamEntity";
        //        throw;
        //    }
        //}

        //public IEnumerable<T> ExecStoreProcedureNoParamEntity(string spQuery)
        //{
        //    try
        //    {
        //        var result = context.Database.SqlQuery<T>(spQuery).ToList();
        //        return result.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Source = "Repository->ExecStoreProcedureNoParamEntity";
        //        throw;
        //    }
        //}

        //public T ExecWithStoreProcedureWithParamEntitySingle(string spQuery, object parameters)
        //{
        //    try
        //    {
        //        return context.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Source = "Repository->ExecWithStoreProcedureWithParamEntitySingle";
        //        throw;
        //    }
        //}


        //public T ExecWithStoreProcedureWithParamEntityMulti(string spQuery, params object[] parameters)
        //{
        //    try
        //    {
        //        return context.Database.SqlQuery<T>(spQuery, parameters).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Source = "Repository->ExecWithStoreProcedureWithParamEntitySingle";
        //        throw;
        //    }
        //}

        //public T ExecStoreProcedureNoParamEntitySingle(string spQuery)
        //{
        //    try
        //    {
        //        return context.Database.SqlQuery<T>(spQuery).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Source = "Repository->ExecStoreProcedureNoParamEntitySingle";
        //        throw;
        //    }
        //}

        public void Save()
        {
            context.SaveChanges();
        }

        public T Get(object key)
        {
            return this.context.Set<T>().Find(key);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList<T>().AsEnumerable();
        }

        public IQueryable<T> Queryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Where(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Any(predicate);
        }
       
        public void Add(T entity)
        {
            this.context.Set<T>().Add(entity);
        }
        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
        }

        public void Edit(T entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }

        //public IList<T> Include(params Expression<Func<T, object>>[] navigationProperties)
        //{
        //    List<T> list;
        //    var query = this._dbSet.AsQueryable();
        //    foreach (var navigationProperty in navigationProperties)
        //        query = query.Include(navigationProperty);
        //    list = query.ToList<T>();
        //    return list;
        //}
        //public IQueryable<T> LazyIncludeNoTracking(params Expression<Func<T, object>>[] navigationProperties)
        //{
        //    var query = this._dbSet.AsQueryable();
        //    foreach (var navigationProperty in navigationProperties)
        //        query = query.Include(navigationProperty);
        //    return query.AsNoTracking<T>();
        //}

        //public IQueryable<T> LazyInclude(params Expression<Func<T, object>>[] navigationProperties)
        //{
        //    var query = this._dbSet.AsQueryable();
        //    foreach (var navigationProperty in navigationProperties)
        //        query = query.Include(navigationProperty);
        //    return query;
        //}
        //public IQueryable<T> SkipTake(int skip, int take)
        //{
        //    return this._dbSet.Skip(skip).Take(take);
        //}

        //public async Task<List<T>> SkipTakeAsync(int skip, int take)
        //{
        //    return await this._dbSet.Skip(skip).Take(take).ToListAsync();
        //}

        //public void Save()
        //{
        //    try
        //    {
        //        context.SaveChanges();
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //    {
        //        Exception raise = dbEx;
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                string message = string.Format("{0}:{1}",
        //                    validationErrors.Entry.Entity.ToString(),
        //                    validationError.ErrorMessage);
        //                // raise a new exception nesting
        //                // the current instance as InnerException
        //                raise = new InvalidOperationException(message, raise);
        //            }
        //        }
        //        throw raise;
        //    }
        //}

        //public async Task<int> SaveAsync()
        //{
        //    int x = -1;
        //    try
        //    {
        //        x = await context.SaveChangesAsync();
        //        return x;
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //    {
        //        Exception raise = dbEx;
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                string message = string.Format("{0}:{1}",
        //                    validationErrors.Entry.Entity.ToString(),
        //                    validationError.ErrorMessage);
        //                // raise a new exception nesting
        //                // the current instance as InnerException
        //                raise = new InvalidOperationException(message, raise);
        //            }
        //        }
        //        throw raise;
        //    }
        //}

        //public int ExecuteStoredProcedureWithoutResult(string query, params object[] parameters)
        //{
        //    return context.Database.ExecuteSqlCommand(query, parameters);
        //}
    }
}
