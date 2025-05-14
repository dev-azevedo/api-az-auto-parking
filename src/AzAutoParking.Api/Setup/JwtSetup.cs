using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AzAutoParking.Api.Setup;

public static class JwtSetup
{
    public static void AddJwtSetup(this IServiceCollection services)
    {   
        var envJwtSecret = Env.GetString("JWT_SECRET");
        var envJwtAudience = Env.GetString("JWT_AUDIENCE");
        var envJwtIssuer = Env.GetString("JWT_ISSUER");
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = envJwtIssuer,
                    ValidAudience = envJwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(envJwtSecret))
                };
            });
    }
}