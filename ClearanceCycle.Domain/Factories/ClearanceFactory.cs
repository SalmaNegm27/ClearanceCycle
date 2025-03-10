using ClearanceCycle.Domain.Entities;
using ClearanceCycle.Domain.Enums;

namespace ClearanceCycle.Domain.Factories
{
    public class ClearanceFactory
    {

        public static ClearanceRequest Create(int companyId,
                                            int employeeId,
                                            DateTime lastWorkingDate,
                                            int clearanceReasonId,
                                            string hrid, string name,
                                            string createdBy, int stepId,
                                            List<int> approvals, string stepName
                                            ,string companyName,string dirHrId,
                                            string secondHrId)
        {

          
            DateTime trimmedLastWorkingDate = TrimSeconds(lastWorkingDate);
            DateTime createdAt = TrimSeconds(DateTime.UtcNow);

            return new ClearanceRequest
            {
                EmployeeId = employeeId,
                ClearanceReasonId = clearanceReasonId,
                LastWorkingDayDate = trimmedLastWorkingDate,
                ResigneeHrId = hrid,
                ResigneeName = name,
                Status = ResignationStatus.New,
                Comment = "",
                CreatedAt = createdAt,
                CreatedBy = createdBy,
                CompanyName = companyName,
                StepApprovalGroup = StepApprovalGroupFactory.Create(stepId, false, approvals, stepName),
                DirectManagerHrid = dirHrId,
                SecondManagerHrId = secondHrId,
                ClearanceHistories = new List<ClearanceHistory>() { new ClearanceHistory { ActionBy = createdBy, ActionAt = createdAt, ActionType = ActionType.Created, Comment = "Request Created Sucessfully",ApprovalGroup= "Created by"+createdBy } }
            };
        }
    private static DateTime TrimSeconds(DateTime date) =>
    new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
    }

}
