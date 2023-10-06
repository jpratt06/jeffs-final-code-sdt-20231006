using Microsoft.AspNetCore.Mvc;
using SharedUtils;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SharedUtils.StringFormatters>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", ([FromServices] StringFormatters formatters) =>
{
    return Results.Ok(formatters.FormatName("Han", "Solo"));
});

app.Run();
