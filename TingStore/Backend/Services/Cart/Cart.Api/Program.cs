// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Cart.Api.Middleware;
using Cart.Application;
using Cart.Core.Repositories;
using Cart.Infrastructure.Repositories;
using HealthChecks.Redis;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddStackExchangeRedisCache(options =>
{
    string redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Can't connect");
    options.Configuration = redisConnectionString;
});




builder.Services.AddMassTransit(config =>
{
    //config.SetKebabCaseEndpointNameFormatter();
    config.UsingRabbitMq((context , configurator) =>
    {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"], c =>
        {
            c.Username(builder.Configuration["EventBusSettings:UserName"]!);
            c.Password(builder.Configuration["EventBusSettings:Password"]!);
        });


        configurator.ConfigureJsonSerializerOptions(options =>
        {
            options.PropertyNameCaseInsensitive = true;
            options.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            options.DefaultIgnoreCondition =  System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            return options;
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddApplication();
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Can't connect"), "Redis Health", HealthStatus.Degraded);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
