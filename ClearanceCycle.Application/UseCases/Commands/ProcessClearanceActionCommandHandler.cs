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
        public ProcessClearanceActionCommandHandler(IWriteRepository writeRepository, IApprovalCycleService approvalCycleService)
        {
            _writeRepository = writeRepository;
            _approvalCycleService = approvalCycleService;

        }
        public async Task<ReponseDto> Handle(ProcessClearanceActionCommand request, CancellationToken cancellationToken)
        {
            var requestHistory = new ClearanceHistory();
            ReponseDto result;


            switch (request.ActionId)
            {
                case 1:
                    var step = await _approvalCycleService.GetCurrentStepWithApprovalGroupIds(request.NextStepId);
                    result = await _writeRepository.ApproveRequest(request, step.Step.ApprovalGroupIds, step.Step.Name);
                    requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, Domain.Enums.ActionType.Approved, "Request Approved", request.RequestId);

                    break;
                case 2:
                    //var validator = new ProcessClearanceActionCommand();
                    //var validateResult = validator.Validate(request);
                    result = await _writeRepository.PendingRequest(request);
                    requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, ActionType.Pending, request.Comment, request.RequestId);
                    break;

                default:
                    throw new ArgumentException("Invalid action type");
            }

              await _writeRepository.AddHistoryAsync(requestHistory);
            return result;

        }
    }
}
