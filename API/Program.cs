using Accessors;
using Engines;
using Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataContracts;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var key = "this_is_a_very_long_and_secure_secret_key_32_chars!!"; //DevOnly
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
//builder.Services.AddScoped<ICustomerEngine, CustomerEngine>();
//builder.Services.AddScoped<ICustomerAccessor,  CustomerAccessor>(sp => new CustomerAccessor(connectionString));
//builder.Services.AddScoped<CustomerManager>();
//builder.Services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();
//builder.Services.AddScoped<IAuthEngine, AuthEngine>();



/************ JWT AUTH CONFIGURATION ************/
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});
builder.Services.AddAuthorization();

/************ CORS CONFIGURATION ************/

//builder.Services.AddScoped<IProductEngine, ProductEngine>();
//builder.Services.AddScoped<IProductAccessor, ProductAccessor>();
//builder.Services.AddScoped<ProductManager>();

//builder.Services.AddScoped<IOrderEngine, OrderEngine>();
//builder.Services.AddScoped<IOrderAccessor, OrderAccessor>(sp => new OrderAccessor(connectionString));
//builder.Services.AddScoped<OrderManager>();

//builder.Services.AddScoped<IAddressEngine, AddressEngine>();
//builder.Services.AddScoped<IAddressAccessor, AddressAccessor>(sp => new AddressAccessor(connectionString));
//builder.Services.AddScoped<AddressManager>();

//builder.Services.AddScoped<ICartEngine, CartEngine>();
//builder.Services.AddScoped<ICartAccessor, CartAccessor>(sp => new CartAccessor(connectionString));
//builder.Services.AddScoped<ICartItemAccessor, CartItemAccessor>();
//builder.Services.AddScoped<CartManager>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173") // Vite
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();

// =====================
// STATIC FILES (PROFILE PICTURES)
// =====================
var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "Images");
uploadDir = Path.GetFullPath(uploadDir); // normalize path

Directory.CreateDirectory(uploadDir);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadDir),
    RequestPath = "/Images"
});

// =====================
// AUTH
// =====================

app.MapPost("/auth/register", (RegisterRequest request, CustomerManager customerManager) =>
{
    try
    {
        int newCustomerId = customerManager.AddCustomer(
            request.Username,
            request.Email,
            request.Password);
        return Results.Created($"/customers/{newCustomerId}", new { id = newCustomerId });
    }
    catch (ArgumentException ex)
    {
        // If the argument exception indicates a duplicate email, return 409 Conflict
        if (ex.Message != null && ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            return Results.Conflict(new { error = ex.Message });

        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("auth/login", (LoginRequest request, CustomerManager customerManager) =>
{
    try
    {
        var token = customerManager.Login(request.Email, request.Password);
        return Results.Ok(new { message = "Login Successful", token });
    }
    catch (ArgumentException)
    {
        // Invalid credentials -> 401 Unauthorized
        return Results.Unauthorized();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/auth/me", (int customerId, CustomerManager customerManager) =>
{
    try
    {
        Customer customer = customerManager.GetCustomer(customerId);

        return Results.Ok(new
        {
            id = customer.Id,
            name = customer.Name,
            email = customer.Email,
            cartId = customer.UserCart,
            paymentMethodId = customer.PaymentMethodId
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});


// =====================
// PRODUCTS
// =====================

app.MapGet("/products", (ProductManager productManager) =>
{
    try
    {
        List<Product> products = productManager.GetAllProducts();
        return Results.Ok(products);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/products/{id:int}", (int id, ProductManager productManager) =>
{
    try
    {
        Product product = productManager.GetProduct(id);
        return Results.Ok(product);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});


// =====================
// CART
// =====================

app.MapGet("/cart", (int customerId, CustomerManager customerManager, CartManager cartManager) =>
{
    try
    {
        Customer customer = customerManager.GetCustomer(customerId);
        Cart cart = cartManager.GetCart(customer.UserCart);
        List<CartItem> items = cartManager.GetCartItems(customer.UserCart);

        return Results.Ok(new
        {
            cart.Id,
            Items = items
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/cart/items", (CartItemRequest request, CartManager cartManager) =>
{
    try
    {
        int cartItemId = cartManager.AddCartItem(request.CartId, request.ProductId, request.Quantity);
        return Results.Ok(new { id = cartItemId });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapDelete("/cart/items/{itemId:int}", (int itemId, CartManager cartManager) =>
{
    try
    {
        cartManager.RemoveCartItem(itemId);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});


// =====================
// ORDERS
// =====================

app.MapPost("/orders", (CheckoutRequest request, OrderManager orderManager) =>
{
    try
    {
        int orderId = orderManager.AddOrder(
            request.CustomerId,
            request.TotalAmount,
            "Pending",
            request.ShippingAddressId,
            request.BillingAddressId);

        return Results.Created($"/orders/{orderId}", new { id = orderId });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/addresses", (AddressDTO request, AddressManager addressManager) =>
    {
        try
        {
            int addressId = addressManager.AddAddress(
                request.CustomerId,
                request.street,
                request.city,
                request.state,
                request.postalCode,
                request.country);
            return Results.Created($"/addresses/{addressId}", new { id = addressId });
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { error = e.Message });
        }
    });

app.MapGet("/orders", (int customerId, OrderManager orderManager) =>
{
    try
    {
        List<Order> orders = orderManager.GetOrdersByCustomer(customerId);
        return Results.Ok(orders);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/orders/{id:int}", (int id, OrderManager orderManager) =>
{
    try
    {
        Order order = orderManager.GetOrder(id);
        return Results.Ok(order);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});


app.Run();

// =====================
// SALES
// =====================

app.MapGet("/sales", (SaleManager saleManager) =>
{
    try
    {
        List<Sale> sales = saleManager.GetAllSales();
        return Results.Ok(sales);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});


// =====================
// DTOs (minimal)
// =====================

public record RegisterRequest(string Username, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record CartItemRequest(int CartId, int ProductId, int Quantity);

public record CheckoutRequest(int CustomerId, decimal TotalAmount, int ShippingAddressId, int BillingAddressId);

public record CreateOrderDTO(int CustomerId, decimal TotalAmount, string OrderStatus, AddressDTO ShippingAddress);

public record AddressDTO(int CustomerId, string street, string city, string state, string postalCode, string country);