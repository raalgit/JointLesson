using DAL;
using Service;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Добавление базовых сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавление сервисов проета
builder.Services.AddIService();
builder.Services.AddIUtility();
builder.Services.AddIRepository();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
