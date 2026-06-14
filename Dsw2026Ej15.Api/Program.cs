using System;
using Dsw2026Ej15.Api.Middleware;
using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();
builder.Services.AddHealthChecks();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapHealthChecks("/health-check");

var persistence = app.Services.GetRequiredService<IPersistence>();

Console.WriteLine("=== SPECIALITIES CARGADAS ===");
foreach (var speciality in persistence.GetAllSpecialities())
{
    Console.WriteLine($"{speciality.Id} - {speciality.Name}");
}
Console.WriteLine("=============================");

app.Run();