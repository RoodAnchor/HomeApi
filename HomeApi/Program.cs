using HomeApi.Configuration;
using HomeApi.Utils;
using HomeApi.Contracts.Validators;
using HomeApi.Data.Repositories;
using HomeApi.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace HomeApi
{
    public class Program
    {
        private static IConfiguration Configuration { get; } =
            new ConfigurationBuilder()
                .AddJsonFile("HomeOptions.json")
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            var connection = Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
            builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
            builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
            builder.Services.AddValidatorsFromAssemblyContaining<AddDeviceRequestValidator>();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddAutoMapper(assembly);
            builder.Services.Configure<HomeOptions>(Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
