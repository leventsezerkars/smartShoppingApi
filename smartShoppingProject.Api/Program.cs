using smartShoppingProject.Api.Middleware;
using smartShoppingProject.Application;
using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Infrastructure;
using smartShoppingProject.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseInfrastructureSerilog();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
