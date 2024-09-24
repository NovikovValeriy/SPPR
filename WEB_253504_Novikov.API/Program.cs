using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WEB_253504_Novikov.API.Data;
using WEB_253504_Novikov.API.Services.VehicleServices;
using WEB_253504_Novikov.API.Services.VehicleTypeServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connStr));
builder.Services.AddTransient<IVehicleService, VehicleService>();
builder.Services.AddTransient<IVehicleTypeService, VehicleTypeService>();

var app = builder.Build();


var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();
await context.Database.MigrateAsync();

await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
