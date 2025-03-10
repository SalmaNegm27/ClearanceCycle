using ClearanceCycle.Domain.Entities;
using ClearanceCycle.Domain.Enums;

namespace ClearanceCycle.Domain.Factories
{
    public class ClearanceHistoryFactory
    {
        public static ClearanceHistory Create(string actionBy ,ActionType actionType,string comment,int requestId ,string approvalGroup)
        {
            return new ClearanceHistory
            {
                ActionBy = actionBy,
                ActionAt = DateTime.UtcNow,
                ActionType = actionType,
                Comment = comment ,
                ClearanceRequestId = requestId,
                ApprovalGroup=approvalGroup
            };
        }
    }
}
