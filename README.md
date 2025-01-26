# Dotnet AppModules

_[![NuGet Version](https://img.shields.io/nuget/v/AppModules)](https://www.nuget.org/packages/AppModules/)_

My take on standardising project structure for .Net minimal API projects.

## Usage

Minimal + chaining

```csharp
// Program.cs

var app = AppModuleBuilder.Create()
    .AddHealthChecksModule()
    .Build();

app.Run();

// HealthChecks.cs

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

```

With setup action

```csharp
WebApplication
    .CreateBuilder(args)
    .AddAppModules(modules =>
    {
        modules.AddHealthChecksModule();
    })
    .Build()
    .Run();
```
