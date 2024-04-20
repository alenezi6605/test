using Microsoft.Extensions.Configuration;
//using TestsService.External.Services;
using Microsoft.Extensions.DependencyInjection;
//using ClientService;
//using MealsService;
//using TestsService.External.Interfaces;

namespace TestsService.External
{
    public static class DependencyInjection
    {
        public static void AddExternal(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddSingleton<IHttpClient, HttpClient>();

            //services.AddSingleton<ISubscriptions, Subscriptions>();

            //services.AddSingleton<IPackages, Packages>();

            //services.AddSingleton<IOrders, Orders>();

            //services.AddSingleton<IMeals, Meals>();

            //services.AddHostedService<ConsumeTestService>();

            //services.AddGrpcClient<ClientRegister.ClientRegisterClient>
            //   (o => o.Address = new Uri(Configuration.GetSection("URLs:ClientUrlHttp2").Value));

            //services.AddGrpcClient<MealRegister.MealRegisterClient>
            //   (o => o.Address = new Uri(Configuration.GetSection("URLs:MealUrlHttp2").Value));
        }

    }
}
