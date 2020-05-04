using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutolotDAL_Core2.EF;
using AutolotDAL_Core2.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutoLotDAL_Core2.Repos
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        private readonly DbSet<T> table;
        private readonly AutoLotContext db;
        protected AutoLotContext Context => this.db;

        public BaseRepo() : this(new AutoLotContext())
        {
        }
        public BaseRepo(AutoLotContext context)
        {
            this.db = context;
            this.table = this.db.Set<T>();
        }

        public Int32 Add(T entity)
        {
            this.table.Add(entity);
            return SaveChanges();
        }

        public Int32 Add(IList<T> entities)
        {
            this.table.AddRange(entities);
            return SaveChanges();
        }

        public Int32 Update(T entity)
        {
            this.table.Update(entity);
            return SaveChanges();
        }
        public Int32 Update(IList<T> entities)
        {
            this.table.UpdateRange(entities);
            return SaveChanges();
        }

        public Int32 Delete(Int32 id, Byte[] time_stamp)
        {
            this.db.Entry(new T() {Id = id, Timestamp = time_stamp}).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Int32 Delete(T entity)
        {
            this.db.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }

        public T GetOne(Int32? id) => this.table.Find(id);

        public virtual List<T> GetAll() => this.table.ToList();
        public List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> order_by, Boolean ascending)
            => (ascending? this.table.OrderBy(order_by) : this.table.OrderByDescending(order_by)).ToList();
        public List<T> GetSome(Expression<Func<T, Boolean>> where)
            => this.table.Where(where).ToList();

        public List<T> ExecuteQuery(String sql) => this.table.FromSqlRaw(sql).ToList();

        public List<T> ExecuteQuery(String sql, Object[] sql_parameters_objects)
            => this.table.FromSqlRaw(sql, sql_parameters_objects).ToList();


        internal Int32 SaveChanges()
        {
            try
            {
                return this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //Thrown when there is a concurrency error
                //for now, just rethrow the exception
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //Thrown when max retries have been hit
                //Examine the inner exception(s) for additional details
                //for now, just rethrow the exception
                throw;
            }
            catch (DbUpdateException ex)
            {
                //Thrown when database update fails
                //Examine the inner exception(s) for additional 
                //details and affected objects
                //for now, just rethrow the exception
                throw;
            }
            catch (Exception ex)
            {
                //some other exception happened and should be handled
                throw;
            }
        }

        public void Dispose()
        {
            this.db?.Dispose();
        }
    }
}