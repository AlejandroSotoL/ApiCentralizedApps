
using CentralizedApps.Data;
using CentralizedApps.FluentValidation;
using CentralizedApps.Middelware;
using CentralizedApps.Profile_AutoMapper;
using CentralizedApps.Repositories;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Net;


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
builder.Services.AddValidatorsFromAssemblyContaining<ProcedureValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DocumentTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<QueryFielValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AvailibityValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MunicipalityValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSportsFacilityvalidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DepartmentValidator>();

builder.Services.AddControllersWithViews();

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
builder.Services.AddAutoMapper(typeof(MunicipalityProfile));

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
// Base de datos
builder.Services.AddDbContext<CentralizedAppsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConectionDefault"),
        sqlOptions => sqlOptions.CommandTimeout(180)
    )
);

// Repositorios y servicios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMunicipalityServices, MunicipalityServices>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
builder.Services.AddScoped<IProcedureServices, ProcedureServices>();
builder.Services.AddScoped<IBank, BankService>();

var app = builder.Build();

// Middleware personalizado de errores
app.UseMiddleware<GlobalExceptionMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CentralizedApps API v1");
    c.RoutePrefix = "swagger";
});
app.UseStaticFiles();
app.UseAuthorization();

// WEB (Razor)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

