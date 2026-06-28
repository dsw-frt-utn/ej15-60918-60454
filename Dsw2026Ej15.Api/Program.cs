using System;
using Dsw2026Ej15.Api.Middleware;
using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Dsw2026Ej15DbContext>(options =>
    options.UseSqlite("Data Source=dsw2026ej15.db"));

builder.Services.AddScoped<IPersistence, PersistenceEf>();

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

using (IServiceScope scope = app.Services.CreateScope())
{
    IPersistence persistence = scope.ServiceProvider.GetRequiredService<IPersistence>();

    Console.WriteLine("=== SPECIALITIES CARGADAS ===");
    foreach (var speciality in persistence.GetAllSpecialities())
    {
        Console.WriteLine($"{speciality.Id} - {speciality.Name}");
    }
    Console.WriteLine("=============================");
}

app.Run();
