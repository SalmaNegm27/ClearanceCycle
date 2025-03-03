using ClearanceCycle.Domain.Entities;
using ClearanceCycle.Domain.Enums;

namespace ClearanceCycle.Domain.Factories
{
    public class ClearanceHistoryFactory
    {
        public static ClearanceHistory Create(string actionBy ,ActionType actionType,string comment,int requestId)
        {
            return new ClearanceHistory
            {
                ActionBy = actionBy,
                ActionAt = DateTime.Now,
                ActionType = actionType,
                Comment = comment ,
                ClearanceRequestId = requestId
            };
        }
    }
}
