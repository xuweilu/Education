using Education.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Education.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll();
        T GetSingle(Guid id);
        Task<T> GetSingleAsync(Guid id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector);
        PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void DeleteGraph(T entity);
        void Save();
        Task SaveAsync();
    }
}
