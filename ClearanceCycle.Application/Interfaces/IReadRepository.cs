﻿using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.UseCases.Queries;
using ClearanceCycle.Domain.Entities;

namespace ClearanceCycle.Application.Interfaces
{
    public interface IReadRepository
    {
        Task<ResultDto<ClearanceDetailsDto>> GetRequestByID(int requestId,int stepId);
        Task<bool> ExistsAsync(int resigneeId, DateTime lastWorkingDay);

        Task<ResultDto<ClearanceRequestsDto>> GetAllRequests(GetClearanceDataTableQuery requestsDto);

        Task<ResultDto<ClearanceReason>> GetReasons();
        Task<ResultDto<RequestHistoryDto>> GetAllRequestHistory(GetRequestHistoryQuery query);
    }

}
