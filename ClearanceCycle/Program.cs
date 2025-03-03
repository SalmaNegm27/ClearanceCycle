using ClearanceCycle.Controllers.ApplicationServices;
using ClearanceCycle.DataAcess.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddApplicationService();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        builder.Services.AddDbContextPool<AuthDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        });
        
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors();

        var app = builder.Build();

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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}