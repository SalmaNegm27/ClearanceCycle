using Azure.Core;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.UseCases.Commands;
using ClearanceCycle.Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClearanceCycle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClearanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClearanceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("AddClearanceRequest")]
        public async Task<IActionResult> AddClearance([FromBody] AddClearanceCommand addResignation)
        {
            // validate on addClearance Command Requet Dto
            var validator = new AddClearanceCommandValidator();
            var validateResult = validator.Validate(addResignation);
            if (!validateResult.IsValid) { return BadRequest(validateResult.Errors.Select(e => e.ErrorMessage)); }

            // send Request to mediator
            var clearanceId = await _mediator.Send(addResignation);
            return Ok(new ReponseDto { Message = "Clearance Added Sucessfully", Success = true });
        }

        [HttpGet("GetClearanceDetails")]
        public async Task<IActionResult> GetRequestById(int id, int stepId)
        {
            var query = new GetClearanceDetailsQuery(id, stepId);
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllClearanceRequests([FromBody] GetClearanceDataTableQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("Approve")]
        public async Task<IActionResult> ApproveRequest([FromBody] ProcessClearanceActionCommand approveClearanceCommand)
        {
            //dynamic command ;
            //switch (approveClearanceCommand.ActionId)
            //{
            //    case 1:
            //        command = new ApproveClearanceCommand
            //        {
            //            NextStepId = approveClearanceCommand.NextStepId,
            //            ApprovalGroupId = approveClearanceCommand.ApprovalGroupId,
            //            RequestId = approveClearanceCommand.RequestId,
            //            ActionBy = approveClearanceCommand.ActionBy,
            //            ActionId = approveClearanceCommand.ActionId
            //        };
            //        break;
            //    case 2:
            //        command = new PendingClearanceCommand
            //        {
            //            ActionBy = approveClearanceCommand.ActionBy,
            //            RequestId = approveClearanceCommand.RequestId,
            //            ApprovalGroupId = approveClearanceCommand.ApprovalGroupId,
            //            NextStepId = approveClearanceCommand.NextStepId,
            //            Comment = "Test"
            //        };
            //        break;
            //        default:
            //        return null;
            //}


            return Ok(await _mediator.Send(approveClearanceCommand));
        }

        [HttpGet("GetAllReasons")]
        public async Task<IActionResult> GetReasons()
        {
            return Ok(await _mediator.Send(new GetClearanceReasonsQuery()));
        }

        [HttpPost("PendingRequest")]
        public async Task<IActionResult> Pending([FromBody] PendingClearanceCommand clearanceCommand)
        {
            // validate on addClearance Command Requet Dto
            var validator = new PendingClearanceCommandValidator();
            var validateResult = validator.Validate(clearanceCommand);
            if (!validateResult.IsValid) { return BadRequest(validateResult.Errors.Select(e => e.ErrorMessage)); }

            // send Request to mediator   
            return Ok(await _mediator.Send(clearanceCommand));
        }

        [HttpPost("UpdateLastWorkingDate")]
        public async Task<IActionResult> Update([FromBody] EditLastWorkingDateCommand editLastWorkingDate)
        {
            // validate on addClearance Command Requet Dto
            var validator = new EditLastWorkingDateCommandValidater();
            var validateResult = validator.Validate(editLastWorkingDate);
            if (!validateResult.IsValid) { return BadRequest(validateResult.Errors.Select(e => e.ErrorMessage)); }

            // send Request to mediator   
            return Ok(await _mediator.Send(editLastWorkingDate));
        }

        [HttpPost("GetAllRequestHistory")]
        public async Task<IActionResult> GetAllClearanceRequests([FromBody] GetRequestHistoryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
