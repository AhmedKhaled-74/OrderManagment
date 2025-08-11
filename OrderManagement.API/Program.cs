using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderManagement.Core.Domain.Identity;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Core.ServiceContracts;
using OrderManagement.Core.Services;
using OrderManagement.Infrastructure.DbContexts;
using OrderManagement.Infrastructure.RepoServices;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Define a CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<string[]>()!)
              .AllowAnyHeader()
              //.WithHeaders("Authorization" , "origin" , "accept" , "content-type")
              //.WithMethods("GET","POST")
              .AllowAnyMethod();
    });
});

//serilog
builder.Host.UseSerilog((context, services
    , loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new ProducesAttribute("application/json"));
    opt.Filters.Add(new ConsumesAttribute("application/json"));

    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    opt.Filters.Add(new AuthorizeFilter(policy));
});


builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Versioning

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Include API versions in response headers
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume default version if none specified
    options.DefaultApiVersion = new ApiVersion(1,0); // Set default API version
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Read version from URL segment
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Format the version as 'v'major[.minor][-status]
    options.SubstituteApiVersionInUrl = true; // Substitute the version in the URL

});
// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

    // Add this if you want to support multiple versions in Swagger
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Order Management API", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo() { Title = "Order Management API", Version = "v2" });
  
});

// Dependency Injection Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepoContract, ItemRepoService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepoContract, OrderRepoService>();

builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderItemRepoContract, OrderItemRepoService>();

builder.Services.AddTransient<IJwtService, JwtService>();

// logging
builder.Services.AddLogging();

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders()
  .AddUserStore<UserStore<AppUser, AppRole, AppDbContext, Guid>>()
  .AddRoleStore<RoleStore<AppRole, AppDbContext, Guid>>()
  .AddRoleManager<RoleManager<AppRole>>()
  .AddUserManager<UserManager<AppUser>>();

//Jwt
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))

    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Use the CORS policy

app.UseCors("AllowAngularClient");

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//swagger

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});

app.Run();
