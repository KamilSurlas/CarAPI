using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CarAPI.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using NLog.Config;
using CarAPI.Models;
using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text;
using CarAPI.IntegrationTests.Helpers;
namespace CarAPI.IntegrationTests
{
    public class CarControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        public CarControllerTests(WebApplicationFactory<Program> factory)
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
        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=10&pageNumber=2")]
        [InlineData("pageSize=15&pageNumber=3")]
        public async Task GetAll_WithQueryParameters_ReturnsOk(string queryParams)
        {
            var response = await _client.GetAsync("/api/car?"+queryParams);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
        [Theory]
        [InlineData("pageSize=1&pageNumber=1")]
        [InlineData("pageSize=11&pageNumber=2")]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAll_WithInvalidQueryParams_ReturnsBadRequest(string queryParams)
        {
            var response = await _client.GetAsync("/api/car?" + queryParams);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_WithValidId_ReturnsOk(int id)
        {
            var response = await _client.GetAsync("/api/car/" + id);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(900)]
        [InlineData(1000)]
        public async Task GetById_WithInvalidId_ReturnsNotFound(int id)
        {

            var response = await _client.GetAsync("/api/car/" + id);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task CreateCar_WithValidModel_ReturnsCreated()
        {
            var model = new NewCarDto()
            {
                BrandName = "Mercedes",
                ModelName = "GLC 220 D 4-MATIC",
                RegistrationNumber = "RegistrationNumberTest",
                ProductionYear = 2019,
                Mileage = 10000,
                BodyType = BodyType.Kombi,
                EngineHorsepower = 194,
                EngineDisplacement = 2143M,
                FuelType = FuelType.Gasoline,
                Drivetrain = Drivetrain.AWD,
                OcInsuranceStartDate = DateTime.Now,
                OcInsuranceEndDate = DateTime.Now.AddYears(1),
                OcPolicyNumber = "PolicyNumberTest"
            };          

            var response =  await _client.PostAsync("api/car", model.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }
        [Fact]
        public async Task CreateCar_WithInvalidModel_ReturnsBadRequest()
        {
            var model = new NewCarDto()
            {
                RegistrationNumber = "RegistrationNumberTest",
                ProductionYear = 2019,
                Mileage = 10000,
                BodyType = BodyType.Kombi,
                EngineHorsepower = 194,
                EngineDisplacement = 2143M,
                FuelType = FuelType.Gasoline,
                Drivetrain = Drivetrain.AWD,
                OcInsuranceStartDate = DateTime.Now,
                OcInsuranceEndDate = DateTime.Now.AddYears(1),
                OcPolicyNumber = "PolicyNumberTest"
            };

            var response = await _client.PostAsync("api/car", model.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task DeleteCar_ForNonExistingCar_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("car/api/9999");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task DeleteCar_ForCarOwner_ReturnsNoContent()
        {
            var car = new Car()
            {
                BrandName = "DeleteTestCarBrandName",
                ModelName = "DeleteTestCarModelName",
                CreatedByUserId = 1              
            };

            SeedCar(car);

            var response = await _client.DeleteAsync("api/car/" + car.Id);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteCar_ForNonCarOwner_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "DeleteTestCarBrandName",
                ModelName = "DeleteTestCarModelName",
                CreatedByUserId = 900
            };

            SeedCar(car);

            var response = await _client.DeleteAsync("api/car/" + car.Id);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
    }
}
