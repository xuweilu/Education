using Education.Abstract;
using Education.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Education.Concrete
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntity, new()
    {
        readonly ApplicationDbContext _entitiesContext;

        public EntityRepository(ApplicationDbContext entitiesContext)
        {
            if (entitiesContext == null)
            {
                throw new ArgumentNullException("entitiesContext");
            }
            _entitiesContext = entitiesContext;
        }
        public virtual IQueryable<T> GetAll()
        {
            return _entitiesContext.Set<T>();
        }
        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = _entitiesContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T GetSingle(Guid id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _entitiesContext.Set<T>().Where(predicate);
        }

        public PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
        {
            return Paginate(pageIndex, pageSize, keySelector, null);
        }

        public PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AllIncluding(includeProperties).OrderBy(keySelector);
            query = (predicate == null)
                ? query
                : query.Where(predicate);
            return query.ToPaginatedList(pageIndex, pageSize);
        }

        public void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            _entitiesContext.Set<T>().Add(entity);
        }

        public void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void DeleteGraph(T entity)
        {
            DbSet<T> dbSet = _entitiesContext.Set<T>();
            dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public void Save()
        {
            _entitiesContext.SaveChanges();
        }
    }
}