using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppModules;

public class AppModuleBuilder(WebApplicationBuilder builder)
{
    private readonly Queue<Action<WebApplication>> _actions = new();

    /// <summary>
    /// The underlying <see cref="WebApplicationBuilder"/>
    /// </summary>
    public WebApplicationBuilder Builder => builder;

    /// <inheritdoc cref="WebApplicationBuilder.Environment"/>
    public IWebHostEnvironment Environment => Builder.Environment;

    /// <inheritdoc cref="WebApplicationBuilder.Services"/>
    public IServiceCollection Services  => Builder.Services;

    /// <inheritdoc cref="WebApplicationBuilder.Configuration"/>
    public ConfigurationManager Configuration  => Builder.Configuration;

    /// <inheritdoc cref="WebApplicationBuilder.Logging"/>
    public ILoggingBuilder Logging  => Builder.Logging;

    /// <inheritdoc cref="WebApplicationBuilder.WebHost"/>
    public ConfigureWebHostBuilder WebHost  => Builder.WebHost;

    /// <inheritdoc cref="WebApplicationBuilder.Host"/>
    public ConfigureHostBuilder Host  => Builder.Host;

    /// <summary>
    /// Registers an action used to configure a <see cref="WebApplication"/>
    /// </summary>
    public AppModuleBuilder ConfigureApp(Action<WebApplication> setupAction)
    {
        _actions.Enqueue(setupAction);
        return this;
    }

    /// <inheritdoc cref="WebApplicationBuilder.Build"/>>
    public WebApplication Build()
    {
        var app = builder.Build();
        while (_actions.TryDequeue(out var action)) action(app);
        return app;
    }

    public static AppModuleBuilder CreateBuilder(string[]? args = null) =>
        new AppModuleBuilder(WebApplication.CreateBuilder(args ?? []));
}

public static class Extensions
{
    public static AppModuleBuilder AddAppModules(
        this WebApplicationBuilder builder,
        Action<AppModuleBuilder>? setupAction = null)
    {
        var moduleBuilder = new AppModuleBuilder(builder);
        setupAction?.Invoke(moduleBuilder);
        return moduleBuilder;
    }
}
