var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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

app.MapPost("/auth/register", (string Username, string Password) =>
{
    //register user
    return success ? Results.Unauthorized() : Results.Ok(user);
});

app.MapPost("/auth/login", (string Username, string Password) =>
{
    //check if login successful
    return success ? Results.Unauthorized() : Results.Ok(user);
});

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

