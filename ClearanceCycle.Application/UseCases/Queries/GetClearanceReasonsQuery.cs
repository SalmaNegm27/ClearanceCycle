using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Domain.Entities;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public record GetClearanceReasonsQuery : IRequest<ResultDto<ClearanceReason>>;
   
}
