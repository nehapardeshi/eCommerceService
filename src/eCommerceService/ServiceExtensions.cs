using eCommerceService.Repositories;
using eCommerceService.Services;
using eCommerceService.Services.Validators;

namespace eCommerceService
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddECommerceServices(this IServiceCollection services)
        {
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IOrdersRepository, OrdersRepository>();
            services.AddTransient<ICustomersRepository, CustomersRepository>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<ICustomersService, CustomersService>();
            services.AddTransient<IOrderValidator, ChangeInOrderItemsValidator>();
            services.AddTransient<IOrderValidator, PayOrderValidator>();
            services.AddTransient<IOrderValidator, ShipOrderValidator>();
            services.AddTransient<IOrderValidator, DeliverOrderValidator>();
            services.AddTransient<IOrderValidator, CancelOrderValidator>();
            
            return services;
        }
    }
}
