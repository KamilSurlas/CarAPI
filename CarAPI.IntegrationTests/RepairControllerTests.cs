using CarAPI.Entities;
using CarAPI.IntegrationTests.Helpers;
using CarAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Xunit;

namespace CarAPI.IntegrationTests
{
    public class RepairControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        public RepairControllerTests(WebApplicationFactory<Program> factory)
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
        private void seedCar(Car car)
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();
            var dbContext = createdScope?.ServiceProvider.GetService<CarDbContext>();

          

            dbContext?.Cars.Add(car);
            dbContext?.SaveChanges();
        }
        private void SeedRepair(Car car, Repair repair)
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();
            var dbContext = createdScope?.ServiceProvider.GetService<CarDbContext>();

            dbContext?.Cars.Add(car);
            car.CarRepairs = new List<Repair>
            {
                repair
            };

           

            dbContext?.Repairs.Add(repair);
            dbContext?.SaveChanges();
        }
        [Fact]
        public async Task GetAllRepairs_ForExistingCar_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
            };

            seedCar(car);

            var response = await _client.GetAsync($"api/car/{car.Id}/repair");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK); 
        }
        [Theory]
        [InlineData(999)]
        [InlineData(null)]
        public async Task GetAllRepairs_ForNonExistingCar_ReturnsNotFound(int id)
        {
            var response = await _client.GetAsync($"api/car/{id}/repair");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task GetRepairById_ForCorrectData_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
            };

            var repair = new Repair()
            {
                CarId = car.Id
            };
        
           SeedRepair(car, repair);

            var response = await _client.GetAsync($"api/car/{car.Id}/repair/{repair.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
        [Theory]
        [InlineData(9999,9999)]
        [InlineData(1,9999)]
        [InlineData(99999,1)]
        public async Task GetRepairById_ForIncorrectData_ReturnsNotFound(int carId, int repairId)
        {          
            var response = await _client.GetAsync($"api/car/{carId}/repair/{repairId}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public async Task CreateRepair_ForExistingCarWithValidModel_ReturnsCreated()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 1
            };

            seedCar(car);

            var repairModel = new NewRepairDto()
            {
                Description = "TestRepair",
                RepairCost = 100.0M,
                RepairDate = DateTime.Now,
            };

            var response = await _client.PostAsync($"api/car/{car.Id}/repair", repairModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }
    }
}
