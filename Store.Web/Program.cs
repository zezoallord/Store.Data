using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.Data.Contexts;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.Dtos;
using Store.Web.Extensions;
using Store.Web.Extenstions;
using Store.Web.Helper;
using Store.Web.MiddelWares;
using System;

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDBContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null));
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("IdentityConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                     policy.AllowAnyMethod().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "http://localhost:45399", "http://localhost:5173");
                });
            });


            var app = builder.Build();
            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
          
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
