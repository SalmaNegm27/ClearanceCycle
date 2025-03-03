using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Entities;
using ClearanceCycle.Domain.Enums;
using ClearanceCycle.Domain.Factories;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    //public class PendingClearanceCommandHandler : IRequestHandler<PendingClearanceCommand, ReponseDto>
    //{
    //    private readonly IWriteRepository _writeRepository;

    //    public PendingClearanceCommandHandler(IWriteRepository writeRepository)
    //    {
    //        _writeRepository = writeRepository;
    //    }
    //    public async Task<ReponseDto> Handle(PendingClearanceCommand request, CancellationToken cancellationToken)
    //    {
    //        var approve = await _writeRepository.PendingRequest(request);
    //        var requestHistory = ClearanceHistoryFactory.Create(request.ActionBy,ActionType.Pending,request.Comment, request.RequestId);
    //        await _writeRepository.AddHistoryAsync(requestHistory);
    //        return approve;
    //    }
    //}
}
