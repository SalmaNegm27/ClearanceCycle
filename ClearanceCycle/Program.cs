using ClearanceCycle.Controllers.ApplicationServices;
 
using Microsoft.EntityFrameworkCore;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ClearanceCycle.Application.Dtos;
using Serilog;
using ClearanceCycle.DataAcess.Middleware;
using ClearanceCycle.DataAcess.Context;
using Hangfire;
using ClearanceCycle.DataAcess.HangFireService;

internal class Program
{
    private static void Main(string[] args)
    {
        
        ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return (true); };
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12 |
                                               SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | (SecurityProtocolType)3072;


        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddApplicationService(builder);
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();

        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            var resignationBL = serviceProvider.GetRequiredService<IHangFireService>();

            var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

            //recurringJobManager.AddOrUpdate("Deactivate Employee Accounts when resigned (New Clearance System) ", () => resignationBL.DeactivateAllEmployeeAccounts(), Cron.Daily());

        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseSwagger(options =>
            //{
            //    options.RouteTemplate = "/openapi/{documentName}.json";
            //});
            //app.MapScalarApiReference();
        }
        app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
        app.UseStaticFiles();
        app.HandleRequestMiddleware();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseHangfireDashboard();


        app.MapControllers();

        app.Run();
    }
}