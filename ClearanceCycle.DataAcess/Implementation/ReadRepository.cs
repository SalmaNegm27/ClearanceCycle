
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
            var request = await _context.ClearanceRequests.Where(c => c.Id == requestId && c.IsCanceled == false && c.IsFinished == false).AsNoTracking()
                                                          .Select(c => new
                                                          {

                                                              c.Id,
                                                              c.ResigneeHrId,
                                                              c.ResigneeName,
                                                              c.ClearanceReason.Reason,
                                                              c.LastWorkingDayDate,
                                                              c.CreatedAt,
                                                              StepName = c.StepApprovalGroup.Name,
                                                              c.Status,
                                                              MajourArea = c.Employee.MajourArea.Name,
                                                              SubDepartment = c.Employee.SubDepartment.Name,
                                                              Department = c.Employee.Department.Name,
                                                              c.Comment,
                                                              FunctionName = c.Employee.Function.Name,
                                                              c.DirectManagerHrid,
                                                              Phone = c.Employee.Phone,
                                                          }).FirstOrDefaultAsync();

            if (request == null) throw new InvalidOperationException(nameof(request));

            var DirectManager = await GetEmployeeName(request.DirectManagerHrid);

            var result = new ClearanceDetailsDto
            {
                Id = request.Id,
                ResigneeHrId = request.ResigneeHrId,
                ResigneeName = request.ResigneeName,
                Comment = request.Comment,
                CreatedAt = request.CreatedAt.ToString("dd-MM-yyyy"),
                MajourArea = request.MajourArea,
                SubDepartment = request.SubDepartment,
                Department = request.Department,
                DirectManager = DirectManager,
                Reason = request.Reason.ToString(),
                LastWorkingDayDate = request.LastWorkingDayDate.ToString("dd-MM-yyyy"),
                Actions = stepActions.Actions,
                FunctionName = request.FunctionName,
                Status = request.Status.ToString(),
                StepApprovalGroupName = request.StepName,
                Phone = request.Phone
            };


            return new ResultDto<ClearanceDetailsDto>
            {

                Data = result,
                Success = true,

            };
        }

        private async Task<string> GetEmployeeName(string Hrid)
        {
            var employee = await _context.Employees.Where(e => e.HrId == Hrid && e.Active).
                Select(e => e.FullName).FirstOrDefaultAsync();
            //if(employee == null)
            //{
            //    throw new InvalidOperationException("Manager Doesn't Exist");
            //}
            return employee;
        }

        #endregion

        #region Get All Request
        public async Task<ResultDto<ClearanceRequestsDto>> GetAllRequests(GetClearanceDataTableQuery query)
        {

            IQueryable<ClearanceRequest> requests = GetClearanceQuey(query).AsNoTracking();

            requests = ApplyGroupFilter(requests, query);

            //var permissions =  GetAllPermissions(query.HrId, query.GroupId);
            // Apply company and major area filter
            requests = ApplyPermissionFilter(requests, query.HrId, query.GroupId);
            // sorting
            if (!string.IsNullOrEmpty(query.SortField))
            {
                query.SortField = query.SortField.Trim();
                bool isAscending = query.SortType?.ToLower() == "asc";
                try
                {
                    requests = isAscending
                        ? requests.OrderByDynamic(query.SortField)
                        : requests.OrderByDescendingDynamic(query.SortField);
                }
                catch
                {
                    requests = requests.OrderBy(c => c.Id);
                }
            }
            // count total requests
            int totalRecords = await requests.CountAsync();

            if (totalRecords == 0)
            {
                return new ResultDto<ClearanceRequestsDto>
                {
                    TotalRecords = 0,
                    Success = true,

                };
            }

            // pagination
            var data = await requests.OrderBy(r=>r.Id).Skip((query.PageNumber - 1) * query.RawsNumber).Take(query.RawsNumber)
                               .Select(c => new ClearanceRequestsDto
                               {
                                   Id = c.Id,
                                   HrId = c.ResigneeHrId,
                                   CurrentStepName = c.StepApprovalGroup.Name,
                                   Status = c.Status.ToString(),
                                   Reason = c.ClearanceReason.Reason,
                                   LastWorkingDayDate = c.LastWorkingDayDate.ToString("dd/MM/yyyy"),
                                   Name = c.ResigneeName,
                                   Department = c.Employee.Department.Name,
                                   Company = c.CompanyName,
                                   currentStepId = c.StepApprovalGroup.CurrentStepId



                               }).ToListAsync();

            string sqlQuery = requests.ToQueryString();
            Console.WriteLine(sqlQuery);

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
                                                    // then include 
                                                    .Include(r => r.StepApprovalGroup.ApprovalGroups)
                                                    .Where(r => !r.IsCanceled && !r.IsFinished);

            if (query.GroupId > 0)
            {
                baseQuery = baseQuery.Where(r => r.StepApprovalGroup.ApprovalGroups
                                                    .Any(x => x.ApprovalGroupId == query.GroupId && x.IsApproved == false));
            }
            return baseQuery;
        }


        private IQueryable<ClearanceRequest> ApplyGroupFilter(IQueryable<ClearanceRequest> requests, GetClearanceDataTableQuery query)
        {
            if (!string.IsNullOrEmpty(query.Filter))
            {
                string filter = query.Filter.Trim();
                requests = requests.Where(x =>
                    x.ResigneeName.Contains(filter) ||
                    x.ResigneeHrId.Contains(filter) ||
                    x.CreatedBy.Contains(filter) ||
                    x.Employee.NationalId.Contains(filter) ||
                    x.ClearanceReason.Reason.Contains(filter));
            }

            return query.GroupId switch
            {
                (int)ApprovalGroups.DirectManager => requests.Where(c => c.DirectManagerHrid == query.HrId),
                (int)ApprovalGroups.SecondManager => requests.Where(c => c.SecondManagerHrId == query.HrId),
                _ => requests
            };
        }
        private IQueryable<ClearanceRequest> ApplyPermissionFilter(IQueryable<ClearanceRequest> requests, string employeeHrId, int? groupId)
        {
            return requests.Where(r => _context.ApprovalGroupEmployees
                .Where(a => a.Employee.HrId == employeeHrId && a.ApprovalGroupId == groupId && a.IsActive)
                .Any(p =>
                    (p.CompanyId == 0 || p.CompanyId == r.Employee.CompanyId) &&
                    (p.MajorAreaId == 0 || p.MajorAreaId == r.Employee.MajourAreaId)));
        }

        private IQueryable<ClearanceRequest> ApplyPermissionFilter(IQueryable<ClearanceRequest> requests, IQueryable<approvalsgroupdto> permissions)
        {
            if (!permissions.Any()) return requests;

            return requests.Where(r => permissions.Any(p =>
                (p.CompanyId == 0 || p.CompanyId == r.Employee.CompanyId) &&
                (p.MajorAreaId == 0 || p.MajorAreaId == r.Employee.MajourAreaId)));
        }

        private IQueryable<approvalsgroupdto> GetAllPermissions(string employeeHrId, int? groupId)
        {
            return  _context.ApprovalGroupEmployees.Where(a => a.Employee.HrId == employeeHrId && a.ApprovalGroupId == groupId && a.IsActive)
                                                .Select(a => new approvalsgroupdto
                                                {
                                                    MajorAreaId = a.MajorAreaId,
                                                    CompanyId = a.CompanyId,
                                                }).AsNoTracking();
        }

        //var companyIds = permissions.Select(p => p.CompanyId).Distinct().ToList();
        //var majorAreaIds = permissions.Select(p => p.MajorAreaId).Distinct().ToList();
        //return requests.Where(r =>
        //     companyIds.Contains(r.Employee.CompanyId) &&
        //     majorAreaIds.Contains(r.Employee.MajourAreaId)); 
        //    return requests.Where(r =>
        //        (companyIds.Contains(0) || companyIds.Contains(r.Employee.CompanyId)) &&
        //        (majorAreaIds.Contains(0) || majorAreaIds.Contains(r.Employee.MajourAreaId)));
        //    return requests
        //        .Where(r => permissions
        //        .Any(p => p.CompanyId == r.Employee.CompanyId && p.MajorAreaId == r.Employee.MajourAreaId)).AsQueryable();
        //}



        #endregion

        #region Check If Request Exist Before
        public async Task<bool> ExistsAsync(int resigneeId)
        {
            return await _context.ClearanceRequests.AnyAsync(c => c.EmployeeId == resigneeId && !c.IsCanceled && !c.IsFinished);
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
                                                        Comment = x.Comment,
                                                        GroupName = x.ApprovalGroup

                                                    }).ToListAsync();

            return new ResultDto<RequestHistoryDto>
            {
                Data = requests,
                Success = true,
                TotalRecords = requests.Count()

            };

        }

        #endregion

        #region Get Approval GroupName With Id
        public async Task<string> GetGroupName(int id)
        {
            if (id == (int)ApprovalGroups.DirectManager)
            { return "Direct Manager"; }
            else if (id == (int)ApprovalGroups.SecondManager)
            { return "Second Manager"; }
            var result = await _context.ApprovalGroups.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
            if (result == null)
                throw new ArgumentNullException("Invalid Approval Group");
            return result.Name;

        }
        #endregion

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

    public record approvalsgroupdto
    {
        public int? MajorAreaId { get; set; }
        public int? CompanyId { get; set; }
    }
    public enum ApprovalGroups
    {
        DirectManager = 1,
        SecondManager = 2,

    }


}
