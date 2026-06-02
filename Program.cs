using Ferreteri.Models;
using Ferreteri.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICategoriasService, CategoriasService>();
builder.Services.AddScoped<IProductosService, ProductosService>();
builder.Services.AddScoped<IMovimientosService, MovimientosService>();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//cadena de conexion a nuestra base de datos
builder.Services.AddDbContext<FerreteriContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Connection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("Connection"))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
