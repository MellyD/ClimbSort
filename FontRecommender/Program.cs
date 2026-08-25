using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClimbSort;
using ClimbSort.Authentication;
using ClimbSort.Automapper;
using ClimbSort.Core.Interfaces;
using ClimbSort.Core.Services;
using ClimbSort.Data;
using ClimbSort.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Client;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

string endpoint = builder.Configuration["AppConfigEndpoint"] ?? "";

#if DEBUG
endpoint = builder.Configuration["AppConfig:Endpoint"] ?? throw new KeyNotFoundException("Configuration value 'AppConfig:Endpoint' is required.");
#endif

var credential = new DefaultAzureCredential(
    new DefaultAzureCredentialOptions()
    {
        ExcludeManagedIdentityCredential = Environment.GetEnvironmentVariable("CONTAINER_APP_NAME") == null
    });

//For this project, we are using Azure App Configuration to store sensitive configuration items.
builder.Configuration.AddAzureAppConfiguration(options => 
{
    options.Connect(new Uri(endpoint), credential)
           .ConfigureKeyVault(kv =>
           {
               kv.SetCredential(credential);
           })
           .Select(KeyFilter.Any, LabelFilter.Null)
           .Select(KeyFilter.Any, "ClimbSort");
});

//Logging is then configured using Serilog.
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration.GetSection("ClimbSort:Serilog")));

//Authentication is configured using Microsoft Identity Web, which allows the application to authenticate users using Azure Active Directory. The authentication settings are fetched from the Azure App Configuration.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessScope", policy =>
    {
        policy.Requirements.Add(new ScopesRequirement("data-access"));
    });
});

// Add Microsoft Identity Web API authentication.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//Database context is initialised using the db connection string, fetched from the Azure App Configuration. The connection string is stored in a Key Vault for extra security.
builder.Services.AddDbContext<ClimbSortDBContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration["ClimbSort:SQLConnectionString"], opt => opt.EnableRetryOnFailure());
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IClimbSortService), typeof(ClimbSortService));
builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration["AutomapperLicenseKey"],typeof(ConfigureAutomapper)); //License key is required for newer version of Automapper. It is fetched from the Azure App Configuration.
builder.Services.Configure<ClimbSortConfig>(builder.Configuration.GetSection("ClimbSort")); //The rest of the application configuration items are fetched and fill the config class (currently empty as no extra config items are yet required).
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the swagger FE for the application when it is in development.
// Also enable HTTPS redirection in development to ensure that the application is accessed securely.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

// Add security headers to the response to enhance security.
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000");
    await next.Invoke();
});

// Requests are logged using Serilog, which catalogues requests that have passed through the system for good historic security and data.
app.UseSerilogRequestLogging();

// CORS policy is applied to allow cross-origin requests from any origin, method, and header. This is useful for development and testing purposes, but should be restricted in production for security reasons.
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseDefaultFiles();
app.UseStaticFiles();

// Security headers are applied to the response to enhance security.
app.UseXContentTypeOptions();
app.UseXfo(opt => opt.Deny());
app.UseReferrerPolicy(opt => opt.SameOrigin());
app.UseCsp(options => options
    .DefaultSources(s => s.Self()
    .CustomSources("data:")
    .CustomSources("https:")
    )
    .ImageSources(s => s.Self().CustomSources("data:","*"))
    .StyleSources(s => s.Self().CustomSources("https://fast.fonts.net").UnsafeInline())
    .FontSources(s => s.Self().CustomSources("https://fast.fonts.net"))
    .ScriptSources(s => s.Self().CustomSources("https://fast.fonts.net").UnsafeInline())
);

app.UseAuthorization();

app.MapControllers();

app.Run();
