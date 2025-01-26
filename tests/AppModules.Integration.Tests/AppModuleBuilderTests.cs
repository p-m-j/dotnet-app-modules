using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Example;

public class AppModuleBuilderTests
{
    [Fact]
    public async Task AppModuleBuilder_configures_services_and_endpoints()
    {
        var client = new WebApplicationFactory<Program>()
            .CreateClient();

        var result = await client.GetAsync("health-check", TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccessStatusCode);
    }
}
