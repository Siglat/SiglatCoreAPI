using Craftmatrix.org.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using SIGLATAPI.Middleware;
using SIGLATAPI.Services;

DotEnv.Load();
var Origin = "_Origin";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add model validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

builder.Services.AddSingleton<IPostgreService, PostgreService>();

builder.Services.AddDbContext<AppDBContext>((serviceProvider, options) =>
{
    var postgreService = serviceProvider.GetRequiredService<IPostgreService>();
    options.UseNpgsql(postgreService.GetConnectionString());
});

builder.Services.AddHostedService<DatabaseInitializer>();
builder.Services.AddHostedService<AdminInitializationService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Origin,
                      policy =>
                      {
                            policy.WithOrigins("http://localhost:8080", "http://localhost:5050", "http://localhost:3000", "http://localhost:3001", "http://localhost:5173", "https://siglatdev.craftmatrix.org", "https://siglat.craftmatrix.org")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});

// Configure API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SIGLAT Core API - Multi-Frontend Emergency Response System",
        Version = "v2.0",
        Description = "Unified API supporting multiple role-based frontends (Citizen, Ambulance, Admin) for comprehensive emergency response management including user authentication, incident reporting, real-time communication, and multi-agency coordination.",
        Contact = new OpenApiContact
        {
            Name = "SIGLAT Team",
            Email = "support@siglat.org",
            Url = new Uri("https://github.com/Siglat")
        },
        License = new OpenApiLicense
        {
            Name = "ISC License",
            Url = new Uri("https://opensource.org/licenses/ISC")
        }
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "Bearer",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new string[]{}
    }});

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
    // Enable annotations for better documentation
    // c.EnableAnnotations(); // Requires Swashbuckle.AspNetCore.Annotations package
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));

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
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors(Origin);
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
