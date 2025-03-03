using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record AddClearanceCommand :IRequest<int>
    {
        public string ResigneeHrId { get; set; }
        public string ResigneeName { get; set; }
        public int ResigneeId { get; set; }
        public int ResignationReasonId { get; set; }
        public DateTime LastWorkingDay { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DirectManagerHrId { get; set; }
        public string DepartmentManagerHrId { get; set; }
    }
}
