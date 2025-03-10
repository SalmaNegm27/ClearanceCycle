using System.Linq.Expressions;

namespace ClearanceCycle.DataAcess.Implementation.WorkFlow
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AuthDbContext _context;

        public GenericRepository(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<T> FindByExpression(Expression<Func<T, bool>> match, List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includes = null, bool trackingEnabled = true)
        {
            IQueryable<T> query = _context.Set<T>().Where(match);

            if (includes != null)
            {
                foreach (var include in includes)
                    query = include(query); // Apply Include and ThenInclude
            }

            return trackingEnabled
                ? await query.FirstOrDefaultAsync()
                : await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }


    }
}
