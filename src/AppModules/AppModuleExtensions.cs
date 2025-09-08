// ReSharper disable MemberCanBePrivate.Global

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppModules;

public static class AppModuleExtensions
{
    private static readonly Queue<Action<WebApplication>> Actions = new();

    public static WebApplicationBuilder AddAppModules(
        this WebApplicationBuilder builder,
        Action<AppModuleBuilder>? setupAction = null)
    {
        var moduleBuilder = new AppModuleBuilder(builder);
        setupAction?.Invoke(moduleBuilder);
        return builder;
    }

    public static WebApplication MapModules(this WebApplication app)
    {
        while (Actions.TryDequeue(out var action)) action(app);
        return app;
    }

    public class AppModuleBuilder(WebApplicationBuilder builder)
    {
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
            Actions.Enqueue(setupAction);
            return this;
        }
    }
}
