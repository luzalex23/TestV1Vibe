using FluentValidation;
using TestV1Vibe.Application;
using TestV1Vibe.Application.DTOs;
using TestV1Vibe.Application.Services.Validators;
using TestV1Vibe.Domain.Entities;
using TestV1Vibe.Domain.Repositories;
using TestV1Vibe.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
// Register repositories
builder.Services.AddScoped<IPlacemarkRepository, PlacemarkRepository>();
builder.Services.AddScoped<IValidator<FilterRequestEntityDto>, FilterRequestValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
