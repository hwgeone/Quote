using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using EntityFramework.Extensions;

namespace RRepository.Implement
{
    public abstract class RepositoryBase<T> where T : class
    {
        private EFDbContext context;
        private readonly DbSet<T> dbset;
        protected IDatabaseFactory DatabaseFactory { get; private set; }
        protected EFDbContext Context { get { return context ?? (context = DatabaseFactory.Get()); } }

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = Context.Set<T>();
        }

        //定义一些通用方法,帮助他的子类实现 IRepository 接口

        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void BatchAdd(T[] entities)
        {
            dbset.AddRange(entities);
        }

        public virtual T AddReturn(T entity)
        {
            T e = dbset.Add(entity);
            return e;
        }

        /// <summary>
        /// 按指定id更新实体,会更新整个实体
        /// </summary>
        /// <param name="identityExp">The identity exp.</param>
        /// <param name="entity">The entity.</param>
        public virtual void Update(Expression<Func<T, object>> identityExp, T entity)
        {
            dbset.AddOrUpdate(identityExp, entity);
        }

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="entity">The entity.</param>
        public virtual void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            dbset.Where(where).Update(entity);
        }

        public virtual void Update(T entity)
        {
            var entry = this.Context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbset.AsQueryable<T>();

        }
        public virtual IQueryable<T> GetBy(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return GetAll();
            }
            return dbset.Where(filter);
        }

        public virtual void DeleteBy(Expression<Func<T, bool>> filter)
        {
            dbset.Where(filter).Delete();
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
