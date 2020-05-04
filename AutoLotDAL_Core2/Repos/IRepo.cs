using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoLotDAL_Core2.Repos
{
    public interface IRepo<T>
    {
        Int32 Add(T entity);
        Int32 Add(IList<T> entities);
        Int32 Update(T entity);
        Int32 Update(IList<T> entities);
        Int32 Delete(Int32 id, Byte[] timeStamp);
        Int32 Delete(T entity);
        T GetOne(Int32? id);
        List<T> GetSome(Expression<Func<T, Boolean>> where);
        List<T> GetAll();
        List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, Boolean ascending);

        List<T> ExecuteQuery(String sql);
        List<T> ExecuteQuery(String sql, Object[] sqlParametersObjects);
    }
}