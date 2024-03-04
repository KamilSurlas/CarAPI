using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.IntegrationTests
{
    public class ProgramClassTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private List<Type> _controllerTypes;
        private WebApplicationFactory<Program> _factory;

        public ProgramClassTests(WebApplicationFactory<Program> factory)
        {
            _controllerTypes = typeof(Program).Assembly.GetTypes().
               Where(t => t.IsSubclassOf(typeof(ControllerBase))).
               ToList();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(c => services.AddScoped(c));
                });
            });
        }

        [Fact]
        public void ConfigureServices_ForControllers_RegistersAllDependencies()
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();

            _controllerTypes.ForEach(t =>
            {
                var controller = createdScope?.ServiceProvider.GetService(t);
                controller.Should().NotBeNull();
            });
        }
    }
}
