using Azure.Identity;
using FontRecommender;
using FontRecommender.Authentication;
using FontRecommender.Automapper;
using FontRecommender.Data;
using FontRecommender.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration["AppConfig:Endpoint"] ?? throw new InvalidOperationException("Configuration value 'AppConfig:Endpoint' is required.");

builder.Configuration.AddAzureAppConfiguration(options => 
{
    options.Connect(connectionString)
           .ConfigureKeyVault(kv =>
           {
               kv.SetCredential(new DefaultAzureCredential());
           })
           .Select(KeyFilter.Any, LabelFilter.Null)
           .Select(KeyFilter.Any, "FontRec");
});

string? check = builder.Configuration["AutomapperLicenseKey"];
string? check2 = builder.Configuration["FontRec:Serilog"];

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration.GetSection("FontRec:Serilog")));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessScope", policy =>
    {
        policy.Requirements.Add(new ScopesRequirement("data-access"));
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<FontRecommendationDBContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("FontRec:SQLConnectionString"));
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration["AutomapperLicenseKey"],typeof(ConfigureAutomapper));
builder.Services.Configure<FontConfig>(builder.Configuration.GetSection("FontRec"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000");
    await next.Invoke();
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseSerilogRequestLogging(); 

app.UseCors("CorsPolicy");
app.UseRouting();
app.UseDefaultFiles();
app.UseStaticFiles();

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
