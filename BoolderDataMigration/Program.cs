using BoolderDataMigration.AutoMapper;
using BoolderDataMigration.Core.Interface;
using BoolderDataMigration.Core.Service;
using BoolderDataMigration.Models;
using FontRecommender;
using FontRecommender.Automapper;
using FontRecommender.Data;
using FontRecommender.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.ComponentModel.DataAnnotations;
using static BoolderDataMigration.Enums;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

//For this project, we are using Azure App Configuration to store sensitive configuration items.
builder.Configuration.AddAzureAppConfiguration(options =>
{
    string connectionString = builder.Configuration["AppConfig:Endpoint"] ?? throw new InvalidOperationException("Configuration value 'AppConfig:Endpoint' is required.");
    options.Connect(connectionString)
           .ConfigureKeyVault(kv =>
           {
               kv.SetCredential(new Azure.Identity.DefaultAzureCredential());
           })
           .Select(KeyFilter.Any, LabelFilter.Null)
           .Select(KeyFilter.Any, "FontRec");
});

//Database context is initialised using the db connection string, fetched from the Azure App Configuration. The connection string is stored in a Key Vault for extra security.
builder.Services.AddDbContext<FontRecommendationDBContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration["FontRec:SQLConnectionString"], opt => opt.EnableRetryOnFailure());
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

//The SqlLite Boolder db path is assembled using the base directory of the application and the name of the database file. This ensures that the database file is located in a consistent location relative to the application.
var dbPath = Path.Combine(
    AppContext.BaseDirectory,
    "boolder.db");

//The context to this database is then initialised.
builder.Services.AddDbContext<BoolderContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlite($"Data Source={dbPath}");
});

//Logging is then configured using Serilog.
builder.Services.AddSerilog((context, configuration) =>
{
    var sectionName = builder.Configuration["FontRec:Serilog"];
    var readerOptions = new Serilog.Settings.Configuration.ConfigurationReaderOptions
    {
        SectionName = string.IsNullOrWhiteSpace(sectionName) ? "Serilog" : sectionName
    };
    configuration.ReadFrom.Configuration(builder.Configuration, readerOptions);
});

builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddTransient(typeof(IMigrationService), typeof(MigrationService));
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration["AutomapperLicenseKey"], typeof(MigrationAutomapper)); //License key is required for newer version of Automapper. It is fetched from the Azure App Configuration.
builder.Services.Configure<FontConfig>(builder.Configuration.GetSection("FontRec"));

using IHost host = builder.Build();

await RunProgram(host.Services);
host.Run();

// The RunProgram method is responsible for executing the main logic of the application.
// It creates a scope for the services, retrieves the IMigrationService, and calls its ScrapeWebsite (Or the other methods in the service class) method to perform the migration.
static async Task RunProgram(IServiceProvider services)
{

    //string bleauHtmlPath = Path.Combine(
    //    AppContext.BaseDirectory,
    //    "bleauAreas.html");
    using var scope = services.CreateScope();
    var provider = scope.ServiceProvider;
    var service = provider.GetService<IMigrationService>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    bool migrationSucceeded = await service?.ScrapeWebsite();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}