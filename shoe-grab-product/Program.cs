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

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 7123, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps("Resources\\server.pfx", "test123", httpsOptions =>
        {
            httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
        });
    });
    options.Listen(IPAddress.Any, 5137, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});

builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(GrpcMappingProfile));

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
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
app.UseCors("AllowAllOrigins");

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();