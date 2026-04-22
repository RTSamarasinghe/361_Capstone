using Microsoft.Extensions.FileProviders;
using Managers;
using Engines;
using Accessors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerEngine, CustomerEngine>();
builder.Services.AddScoped<ICustomerAccessor,  CustomerAccessor>();
builder.Services.AddScoped<CustomerManager>();

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
    int newCustomerId = customerManager.AddCustomer(
        request.Username,
        request.Email,
        request.Password);
    return Results.Created($"/customers/{newCustomerId}", new {id = newCustomerId});
});

app.MapPost("/auth/login", (LoginRequest request) =>
{
    // TODO: validate user + return JWT
    return Results.Ok();
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