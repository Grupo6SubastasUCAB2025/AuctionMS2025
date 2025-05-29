using MSAuction.Application.Handlers;
using MSAuction.Application.Interfaces;
using MSAuction.Application.Services;
using MSAuction.Application.Validators;
using MSAuction.Infraestructure.EventBus.Consumers;
using MSAuction.Infraestructure.Persistence.Repositories;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MSAuction.Infraestructure.Persistence;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var mongoClient = new MongoClient("mongodb://localhost:27017");
var consumerC = new AuctionCreatedConsumer(mongoClient);
_ = Task.Run(() => consumerC.StartListening());
var consumerD = new AuctionUpdatedConsumer(mongoClient);
_ = Task.Run(() => consumerD.StartListening());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auction API", Version = "v1" });
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAuctionHandler).Assembly));
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddValidatorsFromAssemblyContaining<AuctionDtoValidator>();
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddHangfireServer();
builder.Services.AddScoped<IAuctionFinalizer, AuctionFinalizer>();
builder.Services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();
