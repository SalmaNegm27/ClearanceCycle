using ClearanceCycle.DataAcess.Enums;
using ClearanceCycle.DataAcess.Models;
using ClearanceCycle.WorkFlow.Models;
using ClearanceCycle.WorkFlow.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClearanceCycle.WorkFlow.Repositories.Implementation;

public class CycleRepository : ICycleRepository
{
    private readonly AuthDbContext _context;

    public CycleRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Cycle> FindByExpression(Expression<Func<Cycle, bool>> match, Expression<Func<Cycle, object>>[] includes = null
                                                            , bool trackingEnabled = true)
    {
        try
        {
            IQueryable<Cycle> query = _context.Cycles.Where(match);
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return trackingEnabled ? await query.FirstOrDefaultAsync() : await query.AsNoTracking().FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public int GetCycleByCompanyId(int companyId)
    {
        switch (companyId)
        {
            case (int)Companies.EPayment:
                return (int)Cycles.EPayment;
            default:
                return (int)Cycles.Normal;
        }
    }
}
