using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ClearanceCycle.WorkFlow.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> FindByExpression(Expression<Func<T, bool>> match, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includes = null
                                                                        , bool trackingEnabled = true);
        public Task<T> Add(T entity);
    }
}
