namespace Example.Modules.HealthChecks;

public class DatabaseChecker
{
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public async Task<bool> CheckDatabase()
    {
        await Task.Delay(250);
        return true;
    }
}
