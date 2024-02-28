using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
namespace CarAPI.IntegrationTests
{
    public class CarControllerTests
    {
        [Fact]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult()
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/car?pageSize=5&pageNumber=1");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
    }
}
