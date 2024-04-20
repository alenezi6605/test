
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TestsService.AppCore.Common;
using System.Reflection;
using Quartz;
using Microsoft.Extensions.Configuration;
//using TestsService.Respositories.Interfaces;
using TestsService.AppCore.Respositories;
using TestsService.AppCore.Respositories.Interfaces;

namespace TestsService.AppCore
{
    public static class DependencyInjection
    {
        public static void AddAppCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddScoped<IDeliveryOrderRespository, DeliveryOrderRespository>();

            services.AddScoped<IUserRespository, UserRespository>();

            //services.AddScoped<IManufacturingOrderRespository, ManufacturingOrderRespository>();
        }
    }
}
