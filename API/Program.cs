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

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var key = "this_is_a_very_long_and_secure_secret_key_32_chars!!"; //DevOnly

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerEngine, CustomerEngine>();
builder.Services.AddScoped<ICustomerAccessor,  CustomerAccessor>(sp => new CustomerAccessor(connectionString));
builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();
builder.Services.AddScoped<IAuthEngine, AuthEngine>();


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

var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
Directory.CreateDirectory(uploadDir); // ensure it exists

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadDir),
    RequestPath = "/Images"
});

// =====================
// AUTH
// =====================

app.MapPost("auth/register", (RegisterRequest request, CustomerManager customerManager) =>
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

app.MapGet("/auth/me", () =>
{
    // TODO: return current user
    return Results.Ok();
});


// =====================
// PRODUCTS
// =====================

app.MapGet("/products", () =>
{
    // TODO: return all products
    return Results.Ok();
});

app.MapGet("/products/{id:int}", (int id) =>
{
    // TODO: get product by id
    return Results.Ok();
});


// =====================
// CART
// =====================

app.MapGet("/cart", () =>
{
    // TODO: get current user's cart
    return Results.Ok();
});

app.MapPost("/cart/items", (CartItemRequest request) =>
{
    // TODO: add item to cart
    return Results.Ok();
});

app.MapDelete("/cart/items/{itemId:int}", (int itemId) =>
{
    // TODO: remove item from cart
    return Results.NoContent();
});


// =====================
// ORDERS
// =====================

app.MapPost("/orders", () =>
{
    // TODO: checkout cart → create order
    return Results.Ok();
});

app.MapGet("/orders", () =>
{
    // TODO: list user orders
    return Results.Ok();
});

app.MapGet("/orders/{id:int}", (int id) =>
{
    // TODO: get order details
    return Results.Ok();
});


app.Run();


// =====================
// DTOs (minimal)
// =====================

public record RegisterRequest(string Username, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record Product(int Id, string Name);

public record CartItemRequest(int ProductId, int Quantity);