using AzAutoParking.Api.Setup;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
// Add services to the container.

builder.Services.AddDbSetup();
builder.Services.AddOpenApi();
builder.Services.AddDiSetup();
builder.Services.AddFluentValidationSetup();
builder.Services.AddCors();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();