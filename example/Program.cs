using AppModules;
using Example.Modules.Redirects;
using static Example.Modules.HealthChecks.HealthCheckModule;

var builder = WebApplication
    .CreateBuilder()
    .AddAppModules(modules =>
    {
        modules.AddHealthChecksModule()
            .AddRedirectsModule();
    });

builder.Logging.AddSimpleConsole(opt => opt.SingleLine = true);

var app = builder.Build();

app.Run();

public partial class Program;
