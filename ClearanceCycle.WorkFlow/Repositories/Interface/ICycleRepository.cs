using ClearanceCycle.WorkFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.WorkFlow.Repositories.Interface
{
    public interface ICycleRepository
    {
        public Task<Cycle> FindByExpression(Expression<Func<Cycle, bool>> match, Expression<Func<Cycle, object>>[] includes = null
                                                                , bool trackingEnabled = true);

        public int GetCycleByCompanyId(int companyId);  
    }
}
