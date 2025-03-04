using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Core.DataContext;
using Api.Filters;
using Core.Services.Interfaces;
using Core.Services;
using Core.Repositories.Interfaces;
using Core.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Api.HealthChecks;
using Core.Mappings;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    // Console sink for Microsoft logs only
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(logEvent =>
            logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue? source) &&
            source.ToString().Contains("Microsoft"))
        .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<CustomExceptionFilter>();
});

// Add AutoMapper and include your mapping profile
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<RoleMappingProfile>();
    cfg.AddProfile<UserMappingProfile>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dotnet Foundation",
        Version = "v1",
        Description = "API documentation for Dotnet Foundation"
    });

    // Include XML comments if the file exists
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Enable Swagger annotations
    c.EnableAnnotations();

    // Add JWT Authentication Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter your JWT token directly. Example: 'YOUR_ACCESS_TOKEN'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Apply Security Requirement Globally
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
            new List<string>()
        }
    });
});

// Add Health Checks with UI support
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database");

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("Api")));

// Read JWT settings
IConfigurationSection jwtSettings = builder.Configuration.GetSection("JwtSettings");

byte[] key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"));

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Add Authorization Middleware
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Health Checks Endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";  // UI for health checks
});

app.Run();

