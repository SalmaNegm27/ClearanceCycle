using AutoMapper;
using ClearanceCycle.WorkFlow.DTOs;
using ClearanceCycle.WorkFlow.Models;

namespace ClearanceCycle.WorkFlow.HelperMethods
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Step, StepDto>();

            CreateMap<StepAction, ActionDto>()
                .ForPath(dto => dto.Id, memberOptions: o => o.MapFrom(t => t.ActionId))
                .ForPath(dto => dto.Name, memberOptions: o => o.MapFrom(t => t.Action.Name));


        }
    }
}
