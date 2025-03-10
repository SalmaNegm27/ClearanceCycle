using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalSystem.Services.Services.Interface;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Entities;
using ClearanceCycle.Domain.Enums;
using ClearanceCycle.Domain.Factories;
using FluentValidation;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class ProcessClearanceActionCommandHandler : IRequestHandler<ProcessClearanceActionCommand, ReponseDto>
    {
        private readonly IWriteRepository _writeRepository;
        private readonly IApprovalCycleService _approvalCycleService;
        private readonly IReadRepository _readRepository;

        public ProcessClearanceActionCommandHandler(IWriteRepository writeRepository, IApprovalCycleService approvalCycleService, IReadRepository readRepository)
        {
            _writeRepository = writeRepository;
            _approvalCycleService = approvalCycleService;
            _readRepository = readRepository;
        }
        public async Task<ReponseDto> Handle(ProcessClearanceActionCommand request, CancellationToken cancellationToken)
        {
            var requestHistory = new ClearanceHistory();
            ReponseDto result;

            var groupName =await _readRepository.GetGroupName(request.ApprovalGroupId);
            switch (request.ActionId)
            {
                case (int)ActionType.Approved:
                    var step = await _approvalCycleService.GetCurrentStepWithApprovalGroupIds(request.NextStepId);
                    result = await _writeRepository.ApproveRequest(request);
                    requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, Domain.Enums.ActionType.Approved, "Request Approved", request.RequestId, groupName);

                    break;
                case (int)ActionType.Pending:
                    result = await _writeRepository.PendingRequest(request);
                    requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, ActionType.Pending, request.Comment, request.RequestId, groupName);
                    break;

                default:
                    throw new ArgumentException("Invalid action type");
            }

            await _writeRepository.AddHistoryAsync(requestHistory);
            return result;

        }


    }
}
