# AppModules

My take on standardising a project structure for a minimal API project.

## Usage

Minimal + chaining

```csharp
var app = AppModuleBuilder.Create()
    .AddHealthChecksModule()
    .Build();

app.Run();
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
