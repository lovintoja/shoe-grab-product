using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShoeGrabMonolith.Database.Mappers;
using ShoeGrabProductManagement.Contexts;
using ShoeGrabProductManagement.Extensions;
using ShoeGrabProductManagement.Grpc;
using ShoeGrabProductManagement.Mappers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;
using ShoeGrabProductManagement;
using ShoeGrabProductManagement.Services;

var builder = WebApplication.CreateBuilder(args);

//Controllers
builder.Services.AddControllers();

builder.SetupKestrel();

builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(GrpcMappingProfile));

builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

//Contexts
builder.Services.AddDbContextPool<ProductContext>(opt =>
  opt.UseNpgsql(
    builder.Configuration.GetConnectionString("PostgreSQL"),
    o => o
      .SetPostgresVersion(17, 0)));

//Security
builder.Services.AddAuthorization();
builder.AddJWTAuthenticationAndAuthorization();

// Add AutoMapper with all profiles in the assembly
builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
////APP PART////
var app = builder.Build();

//Migrations
app.ApplyMigrations();

app.MapGrpcService<ProductManagementService>();

//Security
app.UseAuthentication();
app.UseAuthorization();

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
}

app.MapControllers();

app.Run();