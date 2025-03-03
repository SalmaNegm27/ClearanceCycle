using ApprovalSystem.Services.Services.Interface;
using AutoMapper;
using ClearanceCycle.DataAcess.Models;
using ClearanceCycle.WorkFlow.DTOs;
using ClearanceCycle.WorkFlow.Models;
using ClearanceCycle.WorkFlow.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ClearanceCycle.WorkFlow.Repositories.Implementation
{
    public class ApprovalCycleService : IApprovalCycleService
    {
        private readonly IGenericRepository<Cycle> _cycleRepo;
        private readonly IGenericRepository<Step> _stepRepo;
        private readonly IMapper _mapper;
        private readonly AuthDbContext _context;


        public ApprovalCycleService(IGenericRepository<Cycle> cycleRepo, IGenericRepository<Step> stepRepo, IMapper mapper,AuthDbContext context)
        {
            _cycleRepo = cycleRepo;
            _stepRepo = stepRepo;
            _mapper = mapper;
            _context = context;
        }

      
        
        //get first step
        //get next step
        //get actions
        public async Task<StepResponseDto> GetFirstStep(int cycleId)
        {
            var result = new StepResponseDto();

            var includes = new List<Func<IQueryable<Cycle>, IIncludableQueryable<Cycle, object>>>
                {
                    query => query.Include(c => c.Steps)

                };
            var cycle = await _cycleRepo.FindByExpression(c => c.Id == cycleId && c.IsActive, includes, false);
            if (cycle != null && cycle.Steps != null && cycle.Steps.Count > 0)
            {

                result.Step = _mapper.Map<StepDto>(cycle.Steps.FirstOrDefault());
                result.Success = true;
                return result;
            }
            result.Message = "Cannot find the cycle or cycle has no steps , Please check you configuration !";
            return result;
        }




        public async Task<StepResponseDto> GetCurrentStepWithApprovalGroupIds(int stepId)
        {
            var result = new StepResponseDto();

            var step = await _stepRepo.FindByExpression(s => s.Id == stepId, null, false);
            if (step != null && step.ApprovalGroupIds != null && step.ApprovalGroupIds != null)
            {
                result.Step = _mapper.Map<StepDto>(step);
                result.Success = true;
                return result;
            }
            result.Message = "Invalid Step Id !";
            return result;

        }
        public async Task<ActionResponseDto> GetCurrentStepActions(int stepId)
        {
            var result = new ActionResponseDto();
            var includes = new List<Func<IQueryable<Step>, IIncludableQueryable<Step, object>>>
                {
                    query => query.Include(s => s.StepActions)
                                                .ThenInclude(a => a.Action)
                };
            var step = await _stepRepo.FindByExpression(s => s.Id == stepId, includes, false);
            if (step != null && step.StepActions != null && step.StepActions.Count > 0)
            {
                result.Actions = _mapper.Map<List<ActionDto>>(step.StepActions);
                result.Success = true;
                return result;
            }

            return result;

        }

    }
}
