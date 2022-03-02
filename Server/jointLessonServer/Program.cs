using DAL;
using JL.DAL;
using JL.DAL.Mongo;
using JL.Settings;
using JL.Utility2L.Abstraction;
using JL.Utility2L.Implementation;
using JL.Utility2L.Models.SignalR;
using jointLessonServer.Middleware;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Service;
using System.Configuration;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Добавление базовых сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(o => o.DefaultScheme = SchemesNamesConst.TokenAuthenticationDefaultScheme);
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, SignalRUserProvider>();

// Добавление и настройка swagger ui
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});

// Добавление контекста базы данных
var environment = Environment.GetEnvironmentVariable("environment") ?? throw new NullReferenceException();
var configuration = getConfiguration(environment);
builder.Services.AddDbContext<JLContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();

// Добавление сервисов проета
builder.Services.AddIService();
builder.Services.AddIUtility();
builder.Services.AddIRepository();

// Регистрация класса настроек
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

// Регистрация класса найтроек соединения к монго
builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);


var app = builder.Build();

app.UseMiddleware<JWTMiddleware>();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AnyOrigin");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalHub>("/signalHub", x =>
    {
    });
});

// Добавление middleware авторизации на основе Jwt
//app.UseMiddleware<JWTMiddleware>();

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