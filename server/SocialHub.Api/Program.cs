using Microsoft.EntityFrameworkCore;
using SocialHub.Infrastructure.Data;
using SocialHub.Infrastructure.Security;
using SocialHub.Core.Abstractions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Retrieve Default connection string
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

// Add services to the container.
builder.Services.AddControllers();

// Automatically registers all validators in the same assembly as Program
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddOptions<PasswordHasherOptions>()
    .Bind(builder.Configuration.GetSection(PasswordHasherOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Database connection (DbContext)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
