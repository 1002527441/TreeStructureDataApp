using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Text.Json.Serialization;
using WebApplication4;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;
var services = builder.Services;

var connectionString = configuration.GetConnectionString("DefaultDb");

services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                               
});

services.AddDbContextFactory<DefaultDbContext>(
       options =>
           options.UseSqlServer(connectionString));


//services.AddDbContext<DefaultDbContext>(options =>
//{
//    options.UseSqlServer(connectionString);
//}, ServiceLifetime.Transient);







var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();

