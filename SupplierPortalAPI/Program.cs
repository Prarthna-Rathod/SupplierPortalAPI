using DataAccess.DataActionContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SupplierPortalAPI.Infrastructure.Builders;
using SupplierPortalAPI.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//Infrastructure=>Builder

builder.BuilderDbContext();
builder.SecutitySchema();
builder.AddSwaggerBuilder();
builder.Services.ConfigureCors();
builder.Services.AddDependancy(builder.Configuration);

//serilog
var _loggrer = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
.CreateLogger();
builder.Logging.AddSerilog(_loggrer);
//Serilog

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCors();

app.UseHttpsRedirection();

app.AddExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
