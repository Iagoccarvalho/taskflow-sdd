using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Api.Middleware;
using TaskFlow.Application;
using TaskFlow.Infrastructure;

const string LocalFrontendCorsPolicy = "LocalFrontend";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var details = context.ModelState
            .SelectMany(entry => entry.Value is null
                ? Enumerable.Empty<string>()
                : entry.Value.Errors.Select(error => error.ErrorMessage))
            .Select(errorMessage => string.IsNullOrWhiteSpace(errorMessage)
                ? "Invalid request data."
                : errorMessage)
            .ToList();

        return new BadRequestObjectResult(new ErrorResponse(
            "Invalid request data.",
            details));
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(LocalFrontendCorsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(LocalFrontendCorsPolicy);

app.MapControllers();

app.Run();
