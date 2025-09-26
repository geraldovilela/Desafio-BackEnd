using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Infrastructure.Repositories.Interfaces
{
    using System.Linq.Expressions;

    namespace RentalApp.Core.Interfaces
    {
        public interface IRepository<T> where T : class
        {
            
            Task<T?> GetByIdAsync<TKey>(TKey id);
            Task<IEnumerable<T>> GetAllAsync();
            Task<T> AddAsync(T entity);
            Task<T> UpdateAsync(T entity);
            Task DeleteAsync<TKey>(TKey id);
            Task<bool> DeleteAsync(T entity);

            
            Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
            Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
            Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

           
            Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
            Task<T?> GetByIdWithIncludesAsync<TKey>(TKey id, params Expression<Func<T, object>>[] includes);
            Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

            
            Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null);

           
            Task<int> CountAsync();
            Task<int> CountAsync(Expression<Func<T, bool>> predicate);

          
            Task<bool> ExistsAsync<TKey>(TKey id);
        }
    }
}
