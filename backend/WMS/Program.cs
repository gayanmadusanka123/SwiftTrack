using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.Interfaces;
using WMS.Services;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
var connectionString = builder.Configuration.GetConnectionString("WMSConnection")
    ?? throw new InvalidOperationException(
        "WMSConnection string is missing"
       );

builder.Services.AddDbContext<WmsDbContext>(
        options => options.UseSqlServer(connectionString)
    );

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
