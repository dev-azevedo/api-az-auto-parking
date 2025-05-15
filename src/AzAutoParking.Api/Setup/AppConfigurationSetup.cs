using DotNetEnv;

namespace AzAutoParking.Api.Setup;

public static class AppConfigurationSetup
{
    public static void AddEnvSetup(this ConfigurationManager configuration)
    {
        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Database:Name"] = Environment.GetEnvironmentVariable("DB_NAME"),
            ["JWT:Secret"] = Environment.GetEnvironmentVariable("JWT_SECRET"),
            ["JWT:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            ["JWT:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            
            ["Email:SmtpHost"] = Environment.GetEnvironmentVariable("EMAIL_SMPTHOST"),
            ["Email:SmtpPort"] = Environment.GetEnvironmentVariable("EMAIL_SMPTPORT"),
            ["Email:From"] = Environment.GetEnvironmentVariable("EMAIL_FROM"),
            ["Email:NameFrom"] = Environment.GetEnvironmentVariable("EMAIL_NAME_FROM"),
            ["Email:Password"] = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
        });
    }
}