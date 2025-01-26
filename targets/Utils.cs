using System.Reflection;
using Semver;
using static SimpleExec.Command;

namespace targets;

public record BuildInfo(string CommitHash, string? BuildId, SemVersion Version);

public static class Utils
{
    private static SemVersion ParseSemanticVersion(this Assembly assembly)
        => SemVersion.Parse(assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion);

    private static async Task<string> GetCommitHash()
        => (await ReadAsync("git", "rev-parse --short HEAD")).StandardOutput.Trim();

    public static async Task<BuildInfo> GetBuildInfo()
    {
        var commitHash = await GetCommitHash();
        var buildId = Environment.GetEnvironmentVariable("Build_BuildId");
        var version = Assembly.GetExecutingAssembly().ParseSemanticVersion()
            .WithMetadata(commitHash);

        if (version.IsPrerelease && buildId != null)
        {
            version = version.WithPrereleaseParsedFrom(version.Prerelease + $".{buildId}");
        }

        return new BuildInfo(
            CommitHash: commitHash,
            BuildId: buildId,
            Version: version);
    }
}
