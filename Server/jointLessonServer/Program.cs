using DAL;
using JL.DAL;
using JL.Settings;
using jointLessonServer.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Service;
using System.Configuration;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Добавление базовых сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(o => o.DefaultScheme = SchemesNamesConst.TokenAuthenticationDefaultScheme);
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddCors();

// Добавление контекста базы данных
var environment = Environment.GetEnvironmentVariable("environment") ?? throw new NullReferenceException();
var configuration = getConfiguration(environment);
builder.Services.AddDbContext<JLContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Добавление сервисов проета
builder.Services.AddIService();
builder.Services.AddIUtility();
builder.Services.AddIRepository();

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

var app = builder.Build();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<JWTMiddleware>();

app.Run();




IConfiguration getConfiguration(string environment)
{
    if (string.IsNullOrEmpty(environment))
        throw new ArgumentNullException(nameof(environment));

    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
    configurationBuilder
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

    switch (environment)
    {
        case "local":
            configurationBuilder.AddJsonFile("appsettings-local.json", true, true);
            break;
        case "test":
            configurationBuilder.AddJsonFile("appsettings-test.json", true, true);
            break;
    }

    return configurationBuilder.Build();
}