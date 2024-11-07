using API;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDBFactory, DBFactory>();
builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantSettingsFactory, TenantSettingsFactory>();
builder.Services.AddHealthChecks().AddCheck<HealthCheck>("HealthCheck");

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x

    .AllowAnyMethod()

    .AllowAnyHeader()

    .SetIsOriginAllowed(origin => true) // allow any origin

    // For a production environmnet, uncomment below method and replace with the domains of each of the tenant
    //.WithOrigins("https://something/StudentAppTenantParis", "https://something/StudentAppTenantGranada", "https://something/StudentAppTenantWarsaw", "https://something/StudentAppTenantLisbon") // Allow only this origin can also have multiple origins separated with comma

    .AllowCredentials()// allow credentials

);

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();