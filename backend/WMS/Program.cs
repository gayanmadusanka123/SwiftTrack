using Microsoft.EntityFrameworkCore;
using WMS.Data;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
var connectionString = builder.Configuration.GetConnectionString("WMSConnection")
    ?? throw new InvalidOperationException(
        "WMSConnection string is missing"
       );

builder.Services.AddDbContext<WmsDbContext>(
        options => options.UseSqlServer(connectionString)
    );

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
