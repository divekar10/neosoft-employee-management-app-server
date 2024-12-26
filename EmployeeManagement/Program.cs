using System.Reflection;
using Carter;
using EmployeeManagement.Entities;
using EmployeeManagement.Features.Common.Generic.Commands;
using EmployeeManagement.Features.Common.Generic.Endpoints;
using EmployeeManagement.Features.Common.Generic.Handlers;
using EmployeeManagement.Features.Common.Generic.Queries;
using EmployeeManagement.Infrastructure.ExceptionHandler;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Infrastructure.OptionsSetup;
using EmployeeManagement.Shared.Constants;
using EmployeeManagement.Shared.Result;
using FluentValidation;
using MediatR;
using Utilities.Content;

var builder = WebApplication.CreateBuilder(args);

ContentLoader.LanguageLoader(Directory.GetCurrentDirectory());

var services = builder.Services;

services.RegisterRepositories();
services.ConfigureServices();
services.ConfigureDatabase(builder.Configuration);
services.ConfigureCors(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.ConfigureOptions<AppSettingsOptionsSetup>();

var assembly = typeof(Program).Assembly;

//builder.Services.AddMediatR(config =>
//    config.RegisterServicesFromAssembly(assembly));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddTransient<IRequestHandler<CreateCommand<Department>, Result<Department>>, CreateCommandHandler<Department>>();
builder.Services.AddTransient<IRequestHandler<UpdateCommand<Department>, Result<Department>>, UpdateCommandHandler<Department>>();
builder.Services.AddTransient<IRequestHandler<DeleteCommand<Department>, Result<bool>>, DeleteCommandHandler<Department>>();
builder.Services.AddTransient<IRequestHandler<GetAllQuery<Department>, Result<IEnumerable<Department>>>, GetAllQueryHandler<Department>>();
builder.Services.AddTransient<IRequestHandler<GetByIdQuery<Department>, Result<Department>>, GetByIdQueryHandler<Department>>();

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
app.MapCarter();


app.Run();
