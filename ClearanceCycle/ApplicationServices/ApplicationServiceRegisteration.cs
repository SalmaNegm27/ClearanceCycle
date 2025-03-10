using ApprovalSystem.Services.Services.Interface;
using AutoMapper;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Application.UseCases.Commands;
using ClearanceCycle.DataAcess.Context;
using ClearanceCycle.DataAcess.HangFireService;
using ClearanceCycle.DataAcess.Implementation;
using ClearanceCycle.DataAcess.Implementation.WorkFlow;
using ClearanceCycle.DataAcess.Middleware;
using ClearanceCycle.WorkFlow.HelperMethods;
using ClearanceCycle.WorkFlow.Repositories.Implementation;
using ClearanceCycle.WorkFlow.Repositories.Interface;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace ClearanceCycle.Controllers.ApplicationServices
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextPool<AuthDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                    //.ToString()).LogTo(Console.WriteLine,LogLevel.Information);

            });
           

            builder.Services.Configure<SystemRequestTokenDto>(builder.Configuration.GetSection("Credentials"));

            services.AddScoped<IWriteRepository, WriteRepository>();
            services.AddScoped<IReadRepository, ReadRepository>();
            services.AddScoped<ICycleRepository, CycleRepository>();
            services.AddHttpClient();

            services.AddScoped<ExternalApilLogger>();
            services.AddScoped<IExternalService, ExternalService>();
            services.AddScoped<IHangFireService, HangeFireService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IApprovalCycleService, ApprovalCycleService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());
            services.AddValidatorsFromAssembly(typeof(AddClearanceCommandValidator).Assembly);


            builder.Host.UseSerilog((context, services, configuration) =>
              configuration.ReadFrom.Configuration(context.Configuration)
                          .WriteTo.File(path: "logs/log-.json",         // File path with a placeholder for the date
                                         rollingInterval: RollingInterval.Day, // Create a new file daily
                                         restrictedToMinimumLevel: LogEventLevel.Information                                    
                                         , formatter: new JsonFormatter()
                                         )
                  
                          .ReadFrom.Services(services));


            services.AddHangfire(c => c
                       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true
               }));
            services.AddHangfireServer();
            return services;
        }

    }


}
