using Azure.Core;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Application.UseCases.Commands;
using ClearanceCycle.Application.UseCases.Queries;
using ClearanceCycle.DataAcess.HangFireService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClearanceCycle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClearanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IExternalService _externalService;
        private readonly IHangFireService _hang;
        private readonly IWriteRepository _writeRepository;
        public ClearanceController(IMediator mediator,IExternalService externalService,IHangFireService hang,IWriteRepository writeRepository)
        {
            _mediator = mediator;
            _externalService = externalService;
            _hang = hang;
            _writeRepository = writeRepository;
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
        [HttpPost("ChangStep")]
        public async Task<IActionResult> ApproveRequest([FromBody] ProcessClearanceActionCommand approveClearanceCommand)
        {
        
            return Ok(await _mediator.Send(approveClearanceCommand));
        }

        [HttpGet("GetAllReasons")]
        public async Task<IActionResult> GetReasons()
        {
            return Ok(await _mediator.Send(new GetClearanceReasonsQuery()));
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

        [HttpGet("GetAllRequestHistory")]
        public async Task<IActionResult> GetAllClearanceRequests([FromQuery]int id)
        {
            var result = await _mediator.Send(new GetRequestHistoryQuery{ RequestId = id });
            return Ok(result);
        }

        [HttpPost("CancelClearanceRequest")]
        public async Task<IActionResult> CancelRequest([FromBody] ProcessClearanceActionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("TestExternalApis")]
        public async Task TestApis()
        {
            //return Ok(await _externalService.GetPortalToken());
            await _hang.ProcessEscalation();
        }
        [HttpPost("AddEscalationManagers")]
        public async Task<IActionResult> AddApprovalWithEscalation([FromBody] ApprovalGroupEmployeesDto request)
        {
            try
            {
                int approvalId = await _writeRepository.AddApprovalWithEscalationAsync(request);
                return Ok(new { Message = "Approval and escalation points added successfully!", ApprovalId = approvalId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("UploadClearanceDocument")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentCommand uploadDocument)
        {

            return Ok(await _mediator.Send(uploadDocument));
        }
    }
}
