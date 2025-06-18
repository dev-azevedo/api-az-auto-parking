using AzAutoParking.Api.Middleware;
using AzAutoParking.Api.Setup;
using AzAutoParking.Api.Utils;
using AzAutoParking.Application.Response;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddEnvSetup();
builder.Services.AddDbSetup(builder.Configuration);
builder.Services.AddDiSetup(builder.Configuration);
builder.Services.AddJwtSetup();
builder.Services.AddAuthorization(options => options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin", "True")));
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddControllers(options => options.Filters.Add<CustomValidationFilter>());
builder.Services.AddFluentValidationSetup();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<JwtAuthenticationMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();