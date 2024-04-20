using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestsService.Infrastructure.Configuration
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContextPool<DatabaseContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("ConnectionString");

                options.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString));
            });

            return services;
        }

    }
}
