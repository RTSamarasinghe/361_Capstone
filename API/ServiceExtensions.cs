
using Accessors;
using Engines;
using Managers;
using Microsoft.AspNetCore.Identity;
using DataContracts;

namespace API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");

        // Customer
        services.AddScoped<ICustomerEngine, CustomerEngine>();
        services.AddScoped<ICustomerAccessor>(_ => new CustomerAccessor(connectionString));
        services.AddScoped<CustomerManager>();
        services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

        // Auth
        services.AddScoped<IAuthEngine, AuthEngine>();

        // Product
        services.AddScoped<IProductEngine, ProductEngine>();
        services.AddScoped<IProductAccessor>(_ => new ProductAccessor(connectionString));
        services.AddScoped<ProductManager>();

        // Order
        services.AddScoped<IOrderEngine, OrderEngine>();
        services.AddScoped<IOrderAccessor>(_ => new OrderAccessor(connectionString));
        services.AddScoped<OrderManager>();

        // Address
        services.AddScoped<IAddressEngine, AddressEngine>();
        services.AddScoped<IAddressAccessor>(_ => new AddressAccessor(connectionString));
        services.AddScoped<AddressManager>();

        // Cart
        services.AddScoped<ICartEngine, CartEngine>();
        services.AddScoped<ICartAccessor>(_ => new CartAccessor(connectionString));
        services.AddScoped<ICartItemAccessor, CartItemAccessor>();
        services.AddScoped<CartManager>();

        //Sales
            services.AddScoped<ISaleEngine, SaleEngine>();
            services.AddScoped<ISaleAccessor>(_ => new SaleAccessor(connectionString));
            services.AddScoped<SaleManager>();

        //SaleItem
        services.AddScoped<ISaleItemEngine, SaleItemEngine>();
        services.AddScoped<ISaleItemAccessor>(_ => new SaleItemAccessor(connectionString));
        services.AddScoped<SaleItemManager>();

        return services;
    }
}

