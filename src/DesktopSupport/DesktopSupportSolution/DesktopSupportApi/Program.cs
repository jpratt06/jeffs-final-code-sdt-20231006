using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/support-tickets", ([FromBody] SupportTicketRequest request) =>
{
    var response = new SupportTicketResponse
    {
        TicketId = Guid.NewGuid(),
        Request = request
    };
    return Results.Ok(response);
});

app.Run();

public record SupportTicketRequest
{
    public string Software { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
}

public record SupportTicketResponse
{
    public Guid TicketId { get; set; }
    public SupportTicketRequest Request { get; set; } = new();
}