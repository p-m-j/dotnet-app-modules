using System.Net;
using AppModules;

namespace Example.Modules.HealthChecks;

internal static class HealthCheckModule
{
    internal static AppModuleBuilder AddHealthChecksModule(this AppModuleBuilder builder)
    {
        builder.Services.AddScoped<HealthChecks>();
        builder.Services.AddScoped<DatabaseChecker>();

        builder.ConfigureApp(app =>
        {
            app.MapGet("/health-check", (HealthChecks module) => module.Index());
        });

        return builder;
    }
}

public class HealthChecks(ILogger<HealthChecks> logger, DatabaseChecker dbChecker)
{
    internal async Task<IResult> Index()
    {
        logger.LogInformation("Beginning health checks");

        var checks = await Task.WhenAll(
            dbChecker.CheckDatabase()
        );

        var payload = new Dictionary<string, object?>
        {
            ["databaseOk"] = checks[0],
        };

        logger.LogInformation("Health check results: {@results}", payload);

        if (checks.Any(x => !x))
        {
            return Results.Problem(
                statusCode: (int)HttpStatusCode.ServiceUnavailable,
                title: "Health check failure",
                extensions: payload);
        }

        return Results.Ok(payload);
    }
}
