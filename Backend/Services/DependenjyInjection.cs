using Repositories;
using Repositories.OrderRepository;
using Repositories.ProductRepository;
using Repositories.UserRepository;

namespace Services {
    public static class DependencyInjection {
        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}