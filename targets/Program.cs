using Microsoft.Extensions.Configuration;
using targets;
using static Bullseye.Targets;
using static SimpleExec.Command;

Target("info", Info);
Target("build", Build);
Target("test", DependsOn("build"), Test);
Target("clean",  () => RunAsync("dotnet", "clean --configuration Release -v quiet"));
Target("pack", DependsOn("info", "clean", "build"), Pack);

Target("run", () => RunAsync("dotnet", "run --project ./example"));

Target("default", DependsOn("info", "build", "test"));

await RunTargetsAndExitAsync(args, ex => ex is SimpleExec.ExitCodeException);

// ReSharper disable once UnusedType.Global
internal static partial class Program
{
    private static readonly IConfiguration Config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("./targets/settings.json")
        .Build();

    private static BuildInfo BuildInfo { get; } = Utils.GetBuildInfo().GetAwaiter().GetResult();

    private static string ProjectName { get; } = Config["ProjectName"]!;

    private static Task Info()
    {
        Console.WriteLine(Directory.GetCurrentDirectory());

        var rows = new List<(string, string)>
        {
            ("Commit Hash", BuildInfo.CommitHash),
            ("Build Id", BuildInfo.BuildId ?? "N/A"),
            ("IsPrerelease", BuildInfo.Version.IsPrerelease ? "Yes" : "No"),
            ("Version", BuildInfo.Version.ToString()),
        };

        var colWidth = rows.Max(x => x.Item1.Length);
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.Item1.PadLeft(colWidth)} : {row.Item2}");
        }

        return Task.CompletedTask;
    }

    private static async Task Build()
    {
        var buildArgs = "build"
                        + " -v quiet"
                        + " --configuration Release"
                        + " --no-incremental"
                        + $" -p:Version={BuildInfo.Version}";

        await RunAsync("dotnet", buildArgs);
    }
    private static async Task Test()
    {
        const string buildArgs = "test"
                                 + " -v quiet"
                                 + " --configuration Release"
                                 + " --no-restore"
                                 + " --no-build"
                                 + " --logger \"console;verbosity=detailed\"";

        await RunAsync("dotnet", buildArgs);
    }

    private static async Task Pack()
    {
        var buildArgs = "pack"
                        + " -v quiet"
                        + " --configuration Release"
                        + " --no-build"
                        + " --output build"
                        + $" -p:PackageVersion={BuildInfo.Version}"
                        + $" ./src/{ProjectName}";


        await RunAsync("dotnet", buildArgs);
    }
}
