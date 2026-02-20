using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using prueba_cajero_api_rest.Application.Handlers;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Entities;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories;
using prueba_cajero_api_rest.Infrastructure.Context;
using prueba_cajero_api_rest.Infrastructure.Repositories;
using prueba_cajero_api_rest.Infrastructure.Settings;
using prueba_cajero_api_rest.Middleware;
using Serilog;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "prueba-cajero")
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Authentication
var jwtSettings = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetSection("Issuer").Value,
            ValidAudience = jwtSettings.GetSection("Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("Key").Value)),
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                var result = ApiResponse<string>.Fail(GlobalErrors.InvalidJwtToken);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(result);
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Prueba Cajero API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "JWT token",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("prueba-cajero"));

// Repositories
builder.Services.AddScoped<IBankRepository, BankRepository>();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AuthenticateCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DepositCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAccountByNumberQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllAccountsQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(WithdrawCommandHandler).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(db);
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Middleware
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.Run();
