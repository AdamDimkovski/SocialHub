using SocialHub.Infrastructure;
using SocialHub.Api;
using SocialHub.Core;
using SocialHub.Api.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCore();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Adds authentication endpoint
var api = app.MapGroup("/api");
api.MapAuthEndpoints();

app.Run();
