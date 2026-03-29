using DentalSystem.Application.Interfaces;
using DentalSystem.Application.Interfaces.Repositories;
using DentalSystem.Application.UseCases.Specialties;
using DentalSystem.Infrastructure.Persistence;
using DentalSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Db config */
// Obtener la connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// registrar el dbContext en el contenedor y a options se le indica que usará sql server
builder.Services.AddDbContext<DentalSystemDbContext>(options =>
    options.UseSqlServer(connectionString));


// registrar el unit of work
// add scoped asegura que la instancia viva sólo durante la petición http.
builder.Services.AddScoped<IUnitOfWork>(provider =>
    provider.GetRequiredService<DentalSystemDbContext>()); // asegura que use la misma instancia exacta del db context

// registrar repositorios
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

// registrar el caso de uso
builder.Services.AddScoped<CreateSpecialtyUseCase>();
builder.Services.AddScoped<GetSpecialtyByIdUseCase>();


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
