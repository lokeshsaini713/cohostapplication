using Business.IServices;
using Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IOC.Extensions
{
    public static class ContainerExtensionService
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterRepositories(configuration);
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IManageService, ManageService>();
        }
    }
}
