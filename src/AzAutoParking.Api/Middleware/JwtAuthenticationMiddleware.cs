using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;

namespace AzAutoParking.Api.Middleware;

public class JwtAuthenticationMiddleware(RequestDelegate next, IJwtService jwtService)
{
    private readonly RequestDelegate _next = next;
    private readonly IJwtService _jwtService = jwtService;

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = _jwtService.ValidateToken(token);

            if (principal != null)
            {
                context.User = principal;
                
                var isResetPassword = principal.HasClaim(c => c.Type == "resetPassword" && c.Value == "True");
                var path = context.Request.Path.Value?.ToLower();
                
                if (isResetPassword && path != null && !path.Contains("password/reset"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Acesso restrito.");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token inválido ou expirado.");
                return;
            }
        }

        await _next(context);
    }
}