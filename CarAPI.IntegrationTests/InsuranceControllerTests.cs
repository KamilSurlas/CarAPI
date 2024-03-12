using Azure;
using CarAPI.Entities;
using CarAPI.IntegrationTests.Helpers;
using CarAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarAPI.IntegrationTests
{
    public class InsuranceControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        public InsuranceControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CarDbContext>));

                        services.Remove(dbContextOptions);
                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services
                         .AddDbContext<CarDbContext>(options => options.UseInMemoryDatabase("CarDb", db => db.EnableNullChecks(false)));

                    });
                });

            _client = _factory.CreateClient();

        }
        private void SeedCar(Car car)
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();
            var dbContext = createdScope?.ServiceProvider.GetService<CarDbContext>();



            dbContext?.Cars.Add(car);
            dbContext?.SaveChanges();
        }     
        [Fact]
        public async Task GetInsurance_ForExistingCar_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "InsuranceTest",
                ModelName = "InsuranceTest",
                OcInsurance = new Insurance()
            };

            SeedCar(car);

            var response = await _client.GetAsync($"api/car/{car.Id}/insurance");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(999)]
        [InlineData(null)]
        public async Task GetInsurance_ForNonExistingCar_ReturnsNotFound(int carId)
        {          
            var response = await _client.GetAsync($"api/car/{carId}/insurance");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task UpdateInsurance_ForExistingCarForCarOwner_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "InsuranceTest",
                ModelName = "InsuranceTest",
                OcInsurance = new Insurance(),
                CreatedByUserId = 1 
            };

            SeedCar(car);

            var response = await _client.PutAsync($"api/car/{car.Id}/insurance", new UpdateInsuranceDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(999)]
        [InlineData(null)]
        public async Task UpdateInsurance_ForNonExistingCar_ReturnsNotFound(int carId)
        {          
            var response = await _client.PutAsync($"api/car/{carId}/insurance", new UpdateInsuranceDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task UpdateInsurance_ForExistingCarNonForCarOwner_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "InsuranceTest",
                ModelName = "InsuranceTest",
                OcInsurance = new Insurance(),
                CreatedByUserId = 2
            };

            SeedCar(car);

            var response = await _client.PutAsync($"api/car/{car.Id}/insurance", new UpdateInsuranceDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
    }
}
