using IntegronERP.Modules.Identity;
using IntegronERP.Modules.Identity.Presentation.Controllers;
using IntegronERP.Api.Extensions;
using FluentValidation;
using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Validators;

var builder = WebApplication.CreateBuilder(args);

// Register Modules
builder.Services.AddIdentityModule(builder.Configuration);

// Add services
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AuthController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();