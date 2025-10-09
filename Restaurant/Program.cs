using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using Restaurant.Repositories;
using Restaurant.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------ CONFIGURAR CONEXIÓN A MySQL ------------------
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 4, 6)) // ajusta la versión según tu MySQL
    ));

// ------------------ REGISTRAR REPOSITORIOS Y SERVICIOS ------------------
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// ------------------ MVC / RAZOR PAGES ------------------
builder.Services.AddRazorPages();

var app = builder.Build();

// ------------------ CONFIGURAR PIPELINE ------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();