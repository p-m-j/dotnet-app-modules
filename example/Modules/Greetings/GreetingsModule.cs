namespace Example.Modules.Greetings;

internal static class GreetingsModule
{
    internal static AppModuleBuilder AddGreetingsModule(this AppModuleBuilder builder)
    {
        // Register module services.
        builder.Services.AddScoped<GreeterService>();

        // Add module endpoints.
        builder.ConfigureApp(app =>
        {
            app.MapGet("/", (GreeterService greeterService) =>
                Results.Ok(greeterService.Greet()));
        });

        return builder;
    }
}

internal class GreeterService(ILogger<GreeterService> logger)
{
    public string Greet(string name = "world")
    {
        logger.LogTrace("Entered method Greet with arguments name={name}", name);
        return $"Hello, {name}";
    }
}
