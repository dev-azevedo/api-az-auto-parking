using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AzAutoParking.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AzAutoParking.Application.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJwtToken(string email, string fullname, long id)
    {
        var secretKey = _configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("id", id.ToString()),
            new Claim("fullname", fullname),
            new Claim("email", email),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}