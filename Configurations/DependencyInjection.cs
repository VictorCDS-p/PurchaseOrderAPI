using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderAPI.Application.Services;
using PurchaseOrderAPI.Infrastructure.Repositories;

namespace PurchaseOrderAPI.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 🔹 Serviços
            services.AddScoped<PurchaseOrderService>();
            services.AddScoped<UserService>();

            // 🔹 Repositórios
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();

            return services;
        }
    }
}