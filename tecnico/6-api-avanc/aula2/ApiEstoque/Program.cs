using ApiEstoque.Infra.Context;
using ApiEstoque.Infra.Repositories;
using ApiEstoque.Infra.Repositories.Interfaces;
using ApiEstoque.Services;
using ApiEstoque.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o DbContext com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.

builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IProdutoDBRepository, ProdutoDBRepository>();
builder.Services.AddScoped<IFabricanteService, FabricanteService>();
builder.Services.
AddScoped<IFabricanteDBRepository, FabricanteDBRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
