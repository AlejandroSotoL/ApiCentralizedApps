using CentralizedApps.Data;
using CentralizedApps.FluentValidation;
using CentralizedApps.HttpClients;
using CentralizedApps.Middelware;
using CentralizedApps.Profile_AutoMapper;
using CentralizedApps.Repositories;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb;
using CentralizedApps.Services.ServicesWeb.Interface;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------- Kestrel ----------------
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5107);
    options.ListenAnyIP(7081, listenOptions => listenOptions.UseHttps());
});

// ---------------- FluentValidation ----------------
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

// ---------------- Controllers / API ----------------
// La API NO exige autenticación global
builder.Services.AddControllers();

// ---------------- MVC (Web) ----------------
// Aquí sí aplicamos autenticación obligatoria
builder.Services.AddControllersWithViews(options =>
{
    var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});

// ---------------- Swagger ----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CentralizedApps API", Version = "v1" });
});

// ---------------- AutoMapper ----------------
builder.Services.AddAutoMapper(typeof(MunicipalityProfile));

// ---------------- Base de datos ----------------
builder.Services.AddDbContext<CentralizedAppsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConectionDefault"),
        sqlOptions => sqlOptions.CommandTimeout(180)
    ));

// ---------------- HttpClient ----------------
builder.Services.AddHttpClient<FintechApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:FintechApi:BaseUrl"]);
});

// ---------------- Repositorios y servicios ----------------
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMunicipalityServices, MunicipalityServices>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPeopleInvitated, PeopleInvitated>();
builder.Services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
builder.Services.AddScoped<IRemidersService, RemindersService>();
builder.Services.AddScoped<IProcedureServices, ProcedureServices>();
builder.Services.AddScoped<IBank, BankService>();
builder.Services.AddScoped<IFintechService, FintechService>();
builder.Services.AddScoped<IWeb, Web>();

// ---------------- Autenticación / Autorización ----------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";       // redirección al login si no hay sesión
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "Admin"));
});

var app = builder.Build();

// ---------------- Middleware ----------------
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// app.UseMiddleware<GlobalExceptionMiddleware>();

// ---------------- Swagger ----------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CentralizedApps API v1");
    c.RoutePrefix = "swagger";
});

// ---------------- Rutas ----------------
// API → libre (no necesita login)
app.MapControllers();

// Web (MVC) → requiere login obligatorio
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
