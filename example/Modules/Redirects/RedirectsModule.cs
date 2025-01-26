namespace Example.Modules.Redirects;

internal static class RedirectsModule
{
    /// <summary>
    /// This isn't very exciting, checkout <see cref="HealthChecks.HealthCheckModule" /> instead.
    /// </summary>
    internal static AppModuleBuilder AddRedirectsModule(this AppModuleBuilder builder) =>
        builder.ConfigureApp(app =>
        {
            app.MapGet("/", () => Results.Redirect("/health-check"));
        });
}
