namespace AppModules;

public class AppModulesTests
{
    [Fact]
    public void Modules_can_register_services()
    {
        const string expected = "A4696F1B-EE32-49B1-A853-E581A9189ABA";
        var app = WebApplication
            .CreateBuilder()
            .AddAppModules(modules => modules.Services.AddSingleton(expected))
            .Build()
            .MapModules();

        var actual = app.Services.GetRequiredService<string>();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Modules_can_configure_web_application()
    {
        const string path = "22B9A680-50C9-470B-A4E5-D322CE27A75E";
        var app = WebApplication
            .CreateBuilder()
            .AddAppModules(modules =>
                modules.ConfigureApp(app => app.MapGet(path, () => Results.Ok()))
            )
            .Build()
            .MapModules();

        var endpoint = (app as IEndpointRouteBuilder).DataSources
            .SelectMany(x => x.Endpoints)
            .OfType<RouteEndpoint>()
            .Single();

        Assert.Equal(path, endpoint.RoutePattern.RawText);
    }
}
