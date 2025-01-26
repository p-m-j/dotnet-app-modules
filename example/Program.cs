using Example.Modules.HealthChecks;
using Example.Modules.Redirects;

var builder = WebApplication
    .CreateBuilder();

builder.Logging.AddSimpleConsole(opt => opt.SingleLine = true);

builder.AddAppModules(modules =>
    modules
        .AddRedirectsModule()
        .AddHealthChecksModule()
);

var app = builder.Build();
app.MapModules();
app.Run();

public partial class Program;
