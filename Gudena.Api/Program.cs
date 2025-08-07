using Gudena.Data;
using Gudena.Data.Entities;
using Gudena.Api.Repositories;
using Gudena.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Gudena.Api.Repositories.Interfaces;
using Gudena.Api.Services.Interfaces;
using Gudena.Data.Repositories;
using Gudena.Services;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Fix datetimes
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(j => j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBuyerProductService, BuyerProductService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
builder.Services.AddScoped<IFavouriteService, FavouriteService>();
builder.Services.AddScoped<IProductReturnRepository, ProductReturnRepository>();
builder.Services.AddScoped<IProductReturnService, ProductReturnService>();
builder.Services.AddScoped<IWarrantyClaimRepository, WarrantyClaimRepository>();
builder.Services.AddScoped<IWarrantyClaimService, WarrantyClaimService>();
builder.Services.AddScoped<IBusinessProductService, BusinessProductService>();
builder.Services.AddScoped<IBusinessOrderRepository, BusinessOrderRepository>();
builder.Services.AddScoped<IBusinessOrderService, BusinessOrderService>();
builder.Services.AddScoped<IBusinessProductReturnRepository, BusinessProductReturnRepository>();
builder.Services.AddScoped<IBusinessProductReturnService, BusinessProductReturnService>();
builder.Services.AddScoped<IBusinessWarrantyClaimRepository, BusinessWarrantyClaimRepository>();
builder.Services.AddScoped<IBusinessWarrantyClaimService, BusinessWarrantyClaimService>();
builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAccountDetailsRepository, AccountDetailsRepository>();
builder.Services.AddScoped<IAccountDetailsService, AccountDetailsService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBusinessProductRepository, BusinessProductRepository>();

// Add Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter token only (without 'Bearer').",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BusinessOnly", policy =>
        policy.RequireClaim("userType", "Business"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BuyerOnly", policy =>
        policy.RequireClaim("userType", "Buyer"));
});

var app = builder.Build();

// Apply migrations if needed (optional)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // dbContext.Database.Migrate(); // Uncomment if you want automatic migrations
}


// Configure the middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
