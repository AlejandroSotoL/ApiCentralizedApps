
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Repositories;

using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using CentralizedApps.FluentValidation;
using CentralizedApps.Data;
using CentralizedApps.Middelware;


var builder = WebApplication.CreateBuilder(args);

// Puerto y HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5107); // HTTP
    options.ListenAnyIP(7081, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS (certificado necesario)
    });
});

// Fluent Validation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CentralizedApps API",
        Version = "v1",
        Description = "Documentación de la API Centralizada"
    });
});

// Base de datos
builder.Services.AddDbContext<CentralizedAppsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConectionDefault"),sql => sql.EnableRetryOnFailure() ));


// Repositorios y servicios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();

var app = builder.Build();

// Middleware personalizado de errores
app.UseMiddleware<GlobalExceptionMiddleware>();

// HTTPS redirection si usas certificado
// app.UseHttpsRedirection();

app.UseStaticFiles();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CentralizedApps API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();

app.MapControllers();

app.Run();

