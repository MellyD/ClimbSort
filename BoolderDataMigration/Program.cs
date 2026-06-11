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

builder.Services.AddDbContext<FontRecommendationDBContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration["FontRec:SQLConnectionString"]);
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

builder.Services.AddDbContext<BoolderContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlite("Data Source=boolder.db");
});

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
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration["AutomapperLicenseKey"], typeof(MigrationAutomapper));
builder.Services.Configure<FontConfig>(builder.Configuration.GetSection("FontRec"));

using IHost host = builder.Build();

await RunProgram(host.Services);
host.Run();

static async Task RunProgram(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var provider = scope.ServiceProvider;
    var service = provider.GetService<IMigrationService>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    bool migrationSucceeded = await service?.MigrateData("G:\\David\\Documents\\Work\\CODE\\clusters.geojson", eDataType.Combine);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}