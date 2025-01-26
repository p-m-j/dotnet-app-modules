# Dotnet AppModules

_[![NuGet Version](https://img.shields.io/nuget/v/AppModules)](https://www.nuget.org/packages/AppModules/)_

My take on standardising project structure for .Net minimal API projects.

## Usage
**All examples use [Implicit Usings](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#implicitusings).

```csharp
// Program.cs

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

// HealthChecks.cs

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
```
