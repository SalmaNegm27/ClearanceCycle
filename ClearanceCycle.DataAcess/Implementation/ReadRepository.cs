using System.Text.RegularExpressions;
using ApprovalSystem.Services.Services.Interface;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Application.UseCases.Queries;
using ClearanceCycle.DataAcess.Models;
using ClearanceCycle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClearanceCycle.DataAcess.Implementation
{
    public class ReadRepository : IReadRepository
    {
        private readonly AuthDbContext _context;
        private readonly IApprovalCycleService _approvalCycleService;
        public ReadRepository(AuthDbContext context, IApprovalCycleService approvalCycleService)
        {
            _context = context;
            _approvalCycleService = approvalCycleService;
        }

        #region get Request By Id
        public async Task<ResultDto<ClearanceDetailsDto>> GetRequestByID(int requestId, int stepId)
        {
            ClearanceCycle.WorkFlow.DTOs.ActionResponseDto stepActions = await _approvalCycleService.GetCurrentStepActions(stepId);
           var request = await _context.ClearanceRequests.Where(c => c.Id == requestId && c.IsCanceled == false && c.IsFinished == false)
                                                         .Select(c => new
                                                         {

                                                             c.Id,
                                                             c.ResigneeHrId,
                                                             c.ResigneeName,
                                                            c.ClearanceReason.Reason,
                                                             c.LastWorkingDayDate,
                                                             c.CreatedAt,
                                                             StepName=c.StepApprovalGroup.Name,
                                                             c.Status,
                                                             MajourArea = c.Employee.MajourArea.Name,
                                                             SubDepartment = c.Employee.SubDepartment.Name,
                                                             Department = c.Employee.Department.Name,
                                                              c.Comment,
                                                             FunctionName = c.Employee.Function.Name,
                                                             c.DirectManagerHrid,
                                                         }).FirstOrDefaultAsync();

            if (request == null) throw new ArgumentNullException(nameof(request));

            var DirectManager = await GetEmployeeName(request.DirectManagerHrid);

            var result = new ClearanceDetailsDto
            {
                Id=request.Id,
                ResigneeHrId = request.ResigneeHrId,
                ResigneeName = request.ResigneeName,
                Comment = request.Comment,
                CreatedAt= request.CreatedAt.ToString("dd-MM-yyyy"),
                MajourArea = request.MajourArea, 
                SubDepartment = request.SubDepartment,
                Department=request.Department,
                DirectManager = DirectManager,
                Reason=request.Reason.ToString(),
                LastWorkingDayDate = request.LastWorkingDayDate.ToString("dd-MM-yyyy"),
                Actions =stepActions.Actions,
                FunctionName =request.FunctionName,
                Status=request.Status.ToString(),   
                StepApprovalGroupName=request.StepName
            };


            return new ResultDto<ClearanceDetailsDto>
            {

                Data = result,
                Success = true,

            };
        }

        private async Task<string> GetEmployeeName(string Hrid)
        {
            return await _context.Employees.Where(e => e.HrId == Hrid && e.Active).Select(e => e.FullName).FirstOrDefaultAsync();
        }

        #endregion

        #region Get All Request
        public async Task<ResultDto<ClearanceRequestsDto>> GetAllRequests(GetClearanceDataTableQuery query)
        {

            IQueryable<ClearanceRequest> requests = GetClearanceQuey(query).AsNoTracking();

            requests = ApplyGroupFilter(requests, query);

          
            if (!string.IsNullOrEmpty(query.SortField))
            {
                query.SortField = query.SortField.Trim();
                bool isAscending = query.SortType?.ToLower() == "ASC";
                requests = isAscending
                    ? requests.OrderByDynamic(query.SortField)
                    : requests.OrderByDescendingDynamic(query.SortField);
            }

            int totalRecords = await requests.CountAsync();

            // pagination

            var data = await requests.Skip((query.PageNumber - 1) * query.RawsNumber).Take(query.RawsNumber)
                               .Select(c => new ClearanceRequestsDto
                               {
                                   Id = c.Id,
                                   HrId = c.ResigneeHrId,
                                   CurrentStepName = c.StepApprovalGroup.Name,
                                   Status = c.Status.ToString(),
                                   Reason = c.ClearanceReason.Reason,
                                   LastWorkingDayDate = c.LastWorkingDayDate.ToString(),
                                   Name = c.ResigneeName,
                                   Department = c.Employee.Department.Name,
                                   Company = c.CompanyName,
                                   currentStepId = c.StepApprovalGroup.CurrentStepId



                               }).ToListAsync();

            return new ResultDto<ClearanceRequestsDto>
            {
                TotalRecords = totalRecords,
                Data = data,
                Success = true,

            };

        }

        private IQueryable<ClearanceRequest> GetClearanceQuey(GetClearanceDataTableQuery query)
        {
            var baseQuery = _context.ClearanceRequests
                                                    .Include(r => r.ClearanceReason)
                                                    .Include(r => r.Employee)
                                                    .Include(r => r.StepApprovalGroup)
                                                    .ThenInclude(r => r.ApprovalGroups).AsQueryable();

            if (query.GroupId > 0)
            {
                baseQuery = baseQuery.Where(r => r.StepApprovalGroup.ApprovalGroups
                                                    .Any(x => x.ApprovalGroupId == query.GroupId  && x.IsApproved == false));
            }
            return baseQuery;
        }


        private IQueryable<ClearanceRequest> ApplyGroupFilter(IQueryable<ClearanceRequest> requests, GetClearanceDataTableQuery query)
        {
            if (!string.IsNullOrEmpty(query.Filter))
            {
                query.Filter = query.Filter.Trim();
                requests = requests.Where(x => x.ResigneeName.Contains(query.Filter) ||
                                         x.ResigneeHrId.Contains(query.Filter) ||
                                         //x.ClearanceReason.ToString().Contains(query.Filter) ||
                                         x.CreatedBy.Contains(query.Filter) ||
                                         x.Employee.NationalId.Contains(query.Filter) ||
                                        x.ClearanceReason.Reason.Contains(query.Filter));
            }

            if (query.GroupId == (int)ApprovalGroups.DirectManager)
            {
                return requests.Where(c => c.DirectManagerHrid == query.HrId);
            }
            else if (query.GroupId == (int)ApprovalGroups.DepartmentManager)
            {
                return requests.Where(c => c.DepartmentManagerHrid == query.HrId);
            }
            return requests;
        }


        #endregion

        #region Check If Request Exist Before
        public async Task<bool> ExistsAsync(int resigneeId, DateTime lastWorkingDay)
        {
            return await _context.ClearanceRequests.AnyAsync(c => c.EmployeeId == resigneeId && c.LastWorkingDayDate == lastWorkingDay && !c.IsCanceled && !c.IsFinished);
        }

        #endregion

        #region Get Clearance Reasons
        public async Task<ResultDto<ClearanceReason>> GetReasons()
        {
            var reasons = await _context.ClearanceReasons.ToListAsync();
            return new ResultDto<ClearanceReason>
            {
                Data = reasons,
                Success = true,
            };
        }

        #endregion

        #region Get All Request history
        public async Task<ResultDto<RequestHistoryDto>> GetAllRequestHistory(GetRequestHistoryQuery query)
        {

            var requests = await _context.ClearanceHistories
                                                    .Where(r => r.ClearanceRequestId == query.RequestId).Select(x => new RequestHistoryDto
                                                    {
                                                        ActionAt = x.ActionAt.ToString("yyyy-MM-dd"),
                                                        ActionBy = x.ActionBy,
                                                        ActionType = x.ActionType.ToString(),
                                                    }).ToListAsync();



            return new ResultDto<RequestHistoryDto>
            {
                Data = requests,
                Success = true,

            };

        }

        #endregion




    }

    public enum ApprovalGroups
    {
        DirectManager = 1,
        DepartmentManager = 2
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string orderByProperty)
        {
            return source.OrderBy(e => EF.Property<object>(e, orderByProperty));
        }

        public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string orderByProperty)
        {
            return source.OrderByDescending(e => EF.Property<object>(e, orderByProperty));
        }
    }

}
