using Carter;
using EmployeeManagement.Entities;
using EmployeeManagement.Features.Common.Generic.Endpoints;
using EmployeeManagement.Infrastructure.ExceptionHandler;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Infrastructure.OptionsSetup;
using EmployeeManagement.Shared.Constants;
using FluentValidation;
using Utilities.Content;

var builder = WebApplication.CreateBuilder(args);

ContentLoader.LanguageLoader(Directory.GetCurrentDirectory());

var services = builder.Services;

services.RegisterRepositories();
services.ConfigureServices();
services.ConfigureDatabase(builder.Configuration);
services.ConfigureCors(builder.Configuration);
services.AddCrudHandlersForEntity<Department>();
services.AddCrudHandlersForEntity<Country>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.ConfigureOptions<AppSettingsOptionsSetup>();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddCarter();
builder.Services.AddAutoMapper(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(Constants.CQRS_KEY);

app.MapGenericCrudEndpoints<Department>();
app.MapGenericCrudEndpoints<Country>();
app.MapCarter();


app.Run();
