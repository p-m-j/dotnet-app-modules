using Example.Modules.Greetings;

var builder = WebApplication
    .CreateBuilder()
    .AddAppModules(modules =>
    {
        modules.AddGreetingsModule();
    });

var app = builder.Build();
app.MapModules();
app.Run();
