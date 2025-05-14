using AzAutoParking.Api.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddEnvSetup();
builder.Services.AddDbSetup(builder.Configuration);
builder.Services.AddJwtSetup();
builder.Services.AddDiSetup();
builder.Services.AddFluentValidationSetup();

builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();