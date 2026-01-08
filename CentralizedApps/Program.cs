using CentralizedApps.Data;
using CentralizedApps.FluentValidation;
using CentralizedApps.HttpClients;
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
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

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
    // Add global CSRF validation for MVC
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
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
// Connection string from environment or config
var connectionString = builder.Configuration.GetConnectionString("ConectionDefault");

// Replace environment variable placeholders if present
connectionString = connectionString?
    .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER") ?? "")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");

builder.Services.AddDbContext<CentralizedAppsDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions => sqlOptions.CommandTimeout(180)
    ));

// ---------------- HttpClient ----------------
var fintechUrl = builder.Configuration["ExternalApis:FintechApi:BaseUrl"]?
    .Replace("${FINTECH_API_URL}", Environment.GetEnvironmentVariable("FINTECH_API_URL") ?? "");

builder.Services.AddHttpClient<FintechApiClient>(client =>
{
    client.BaseAddress = new Uri(fintechUrl ?? "https://localhost");
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
builder.Services.AddScoped<IGeneralProcedures, GeneralProcedures>();
builder.Services.AddScoped<IGeneralMunicipality, GeneralMunicipality>();
builder.Services.AddScoped<IWeb, Web>();

// ---------------- Rate Limiting ----------------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Login rate limiting: max 5 attempts per minute per IP
    options.AddPolicy("LoginPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    // General API rate limiting
    options.AddPolicy("ApiPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 2
            }));
});

// ---------------- Health Checks ----------------
builder.Services.AddHealthChecks()
    .AddDbContextCheck<CentralizedAppsDbContext>("database");

// ---------------- Autenticación / Autorización ----------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(45);
        options.SlidingExpiration = true;
        // Security: HttpOnly and Secure cookies
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Allows HTTP in dev, HTTPS in prod
        options.Cookie.SameSite = SameSiteMode.Lax; // Changed from Strict to allow redirect after login
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "Admin"));
});

// ---------------- Antiforgery ----------------
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Allows HTTP in dev
});

var app = builder.Build();

// ---------------- Security Headers ----------------
app.Use(async (context, next) =>
{
    // Prevent clickjacking
    context.Response.Headers["X-Frame-Options"] = "DENY";
    // Prevent MIME type sniffing
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    // XSS Protection
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    // Referrer Policy
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    // Content Security Policy (adjust as needed)
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net; font-src 'self' https://fonts.gstatic.com; img-src 'self' data: https:;";

    await next();
});

// ---------------- HTTPS Redirection (Production) ----------------
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// ---------------- Rate Limiting ----------------
app.UseRateLimiter();

// ---------------- Middleware ----------------
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Uncomment for global exception handling in production
// app.UseMiddleware<GlobalExceptionMiddleware>();

// ---------------- Health Check ----------------
app.MapHealthChecks("/health");

// ---------------- Swagger (Development Only) ----------------
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CentralizedApps API v1");
        c.RoutePrefix = "swagger";
    });
}

// ---------------- Rutas ----------------
// API → libre (no necesita login)
app.MapControllers();

// Web (MVC) → requiere login obligatorio
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();