using GerenciadorTarefasApi.Infra.Mapping;
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefasApi.Infra.Mapping;
using GerenciadorTarefasApi.Services.Interfaces;
using GerenciadorTarefasApi.Infra.Repositories; 
using GerenciadorTarefasApi.Infra.Repositories.Interfaces;
using GerenciadorTarefasApi.Services; 
using GerenciadorTarefasApi.Services.Interfaces; 
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefasApi.Infra.Context;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<ITarefaRepository, TarefaDBRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioDBRepository>();
builder.Services.AddScoped<ITagRepository, TagDBRepository>();


builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>(); 
builder.Services.AddScoped<ITagService, TagService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfile)); 


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
