using BugTrackerApi.Services;

using Marten;

using Microsoft.OpenApi.Models;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new List<string>()
    }
});
});

builder.Services.AddSingleton<ISystemTime, SystemTime>();
builder.Services.AddScoped<BugReportManager>();
builder.Services.AddScoped<SoftwareCatalogManager>();
builder.Services.AddScoped<SlugUtils.SlugGenerator>();
builder.Services.AddAuthentication().AddJwtBearer();

var connectionString = builder.Configuration.GetConnectionString("bugs") ?? throw new Exception("Need A Connection String");
builder.Services.AddMarten(cfg =>
{
    cfg.Connection(connectionString);

    cfg.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;

}).UseLightweightSessions();

var desktopSupportUrl = builder.Configuration.GetValue<string>("desktop-support") ?? throw new Exception("Need a URL for Desktop Support");

builder.Services.AddHttpClient<IDesktopSupportHttpClient, DesktopSupportHttpClient>(client =>
{
    client.BaseAddress = new Uri(desktopSupportUrl);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
// Right here!
app.Run(); // Kestrel will start and listen for incoming requests.

public partial class Program { } // did this for Alba.