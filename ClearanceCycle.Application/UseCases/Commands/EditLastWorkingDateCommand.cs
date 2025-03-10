using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record EditLastWorkingDateCommand :IRequest<ReponseDto>
    {
        public int RequestId { get; set; }
        public DateTime LastWorkingDay { get; set; } = DateTime.UtcNow;
        public string ActionBy { get; set; }
        public EditLastWorkingDateCommand(int requestId,DateTime lastWorkingDay,string actionBy)
        {
            RequestId = requestId;
            LastWorkingDay = lastWorkingDay;
            ActionBy = actionBy;
        }

    }
}
