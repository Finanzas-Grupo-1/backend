using finanzas_project.IAM.Infrastructure.Tokens.JWT.Services;
using finanzas_project.Shared.Interfaces.ASP.Configuration;
using finanzas_project.IAM.Application.Internal.CommandServices;
using finanzas_project.IAM.Application.Internal.OutboundServices;
using finanzas_project.IAM.Application.Internal.QueryServices;
using finanzas_project.IAM.Domain.Repositories;
using finanzas_project.IAM.Domain.Services;
using finanzas_project.IAM.Infrastructure.Hashing.BCrypt.Services;
using finanzas_project.IAM.Infrastructure.Persistence.EFC.Repositories;
using finanzas_project.IAM.Infrastructure.Tokens.JWT.Configuration;
using finanzas_project.IAM.Interfaces.ACL.Services;
using finanzas_project.IAM.Interfaces.ACL;
using finanzas_project.Shared.Domain.Repositories;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Configuration;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using finanzas_project.BonusesManagement.Domain.Repositories;
using finanzas_project.BonusesManagement.Infrastructure.Persistence.EFC;
using finanzas_project.BonusesManagement.Domain.Services;
using finanzas_project.BonusesManagement.Application.Internal.CommandServices;
using finanzas_project.BonusesManagement.Application.Internal.QueryServices;
using finanzas_project.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using finanzas_project.BonusesManagement.Application.Calculations;



var builder = WebApplication.CreateBuilder(args);





// Add services to the container.
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Database Context and Logging Levels

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error)
                    .EnableDetailedErrors();
    });




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "ACME.LearningCenterPlatform.API",
                Version = "v1",
                Description = "ACME Learning Center Platform API",
                TermsOfService = new Uri("https://acme-learning.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "ACME Studios",
                    Email = "contact@acme.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });
// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedAllPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBonusesRepository, BondRepository>();
builder.Services.AddScoped<IBondCommandService,BondCommandService>();
builder.Services.AddScoped<IBondQueryService,BondQueryService>();
builder.Services.AddScoped<IBondValuation, BondValuationDomainService>();


// IAM Bounded Context Injection Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

var app = builder.Build();

// Verify Database Objects area Created

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS Policy
app.UseCors("AllowedAllPolicy");

// Add Authorization Middleware to the Request Pipeline

app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
