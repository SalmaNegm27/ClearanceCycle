using ApprovalSystem.Services.Services.Interface;
using AutoMapper;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Application.UseCases.Commands;
using ClearanceCycle.DataAcess.Implementation;
using ClearanceCycle.DataAcess.Implementation.WorkFlow;
using ClearanceCycle.WorkFlow.HelperMethods;
using ClearanceCycle.WorkFlow.Repositories.Implementation;
using ClearanceCycle.WorkFlow.Repositories.Interface;
using FluentValidation;

namespace ClearanceCycle.Controllers.ApplicationServices
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IWriteRepository, WriteRepository>();
            services.AddScoped<IReadRepository, ReadRepository>();
            services.AddScoped<ICycleRepository, CycleRepository>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IApprovalCycleService, ApprovalCycleService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());
            services.AddValidatorsFromAssembly(typeof(AddClearanceCommandValidator).Assembly);

            return services;
        }

    }


}
