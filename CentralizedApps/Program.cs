using CentralizedApps.Domain.Interfaces;
using CentralizedApps.Infrastructure.Data;
using CentralizedApps.Infrastructure.Repositories;
using CentralizedApps.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using CentralizedApps.Domain.Entities;
using CentralizedApps.Infrastructure.FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{                                                                                                               
    options.ListenAnyIP(5107); // HTTP
    options.ListenAnyIP(7081, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS (necesitas certificado)
    });
});

//FLUET VALIDATION
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configuración de Swagger
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

// Configuración de la base de datos
builder.Services.AddDbContext<CentralizedAppsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConectionDefault")));

// Repositorios y Unidad de Trabajo
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Activar Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CentralizedApps API v1");
    c.RoutePrefix = "swagger";// Esto es clave
});

app.UseStaticFiles();
// Redirección HTTPS
//app.UseHttpsRedirection();

// Mapear controladores
app.MapControllers();

app.Run();
