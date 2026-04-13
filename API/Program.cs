var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<ICustomerAccessor, CustomerAccesor>();
builder.Services.AddScoped<ICustomerEngine, CustomerEngine>();
builder.Services.AddScoped<ICustomerManager, CustomerManager>();


var key = "super_secret_key_12345";

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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Auth functionality

app.MapPost("/auth/register", (RegisterRequest request, ICustomerManager customerManager) =>
{
    //A UUID would have been more prefferable here
    var existingUser = customerManager.GetCustomerByEmail(request.Email);

    if (existingUser != null)
    {
        return Results.Unauthorized();
    }

    int newId = customerManager.AddCustomer(
        request.Username,
        request.Email,
        request.Password, // Use the hash here in production
        0, // Default CartId
        0  // Default PaymentMethodId
    );

    if (newId > 0)
    {
        // Return 201 Created with the new ID
        return Results.Created($"/auth/customer/{newId}", new { Id = newId, Email = request.Email });
    }

    return Results.BadRequest("Registration failed.");

});

public record RegisterRequest(string Username, string Email, string Password);

app.MapPost("/auth/login", (LoginRequest request, ICustomerManager cutomerManager) =>
{
    //check if login successful
    var user = customerManager.GetCustomerByEmail(request.Email);

    //Validate user existence
    if (user == null || user.PassHash != request.Password
    {
        return Results.Unauthorized();
    })

    return Results.Ok(user);
});

// A simple DTO for the request body
public record LoginRequest(string Email, string Password);

app.MapPost("/auth/me", () =>
{
    //check who curr user is
    return success ? Results.Unauthorized() : Results.Ok(user);
});

// Product Functionality

app.MapGet("/products", () =>
{
    return Results.Ok();
});

app.MapGet("/products/{id}", (int id) =>
{
    //var product = getProduct(id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/products", (Product product) =>
{
    //Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

// Cart Functionality

app.MapGet("/cart", () =>
{
    return Results.Ok();
});

app.MapPost("/cart/items", (int CartItemId) =>
{
    //AddItem(item);
    return Results.Ok();
});

app.MapDelete("/cart", () =>
{
    //ClearCart();
    return Results.NoContent();
});




app.Run();

