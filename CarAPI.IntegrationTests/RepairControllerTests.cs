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
        private void AssignRepairToCar(Car car, Repair repair)
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
        private void SeedRepair(Repair repair)
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();
            var dbContext = createdScope?.ServiceProvider.GetService<CarDbContext>();



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
        
           AssignRepairToCar(car, repair);

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
        [Fact]
        public async Task CreateRepair_ForNonExistingCarWithValidModel_ReturnsNotFound()
        {
           
            var repairModel = new NewRepairDto()
            {
                Description = "TestRepair",
                RepairCost = 100.0M,
                RepairDate = DateTime.Now,
            };
            var response = await _client.PostAsync($"api/car/{999}/repair", repairModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task CreateRepair_ForNonCarOwnerWithValidModel_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 2
            };

            seedCar(car);
            var repairModel = new NewRepairDto()
            {
                Description = "TestRepair",
                RepairCost = 100.0M,
                RepairDate = DateTime.Now,
            };
            var response = await _client.PostAsync($"api/car/{car.Id}/repair", repairModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task CreateRepair_ForExistingCarWithInvalidModel_ReturnsBadRequest()
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
                RepairDate = DateTime.Now,
            };

            var response = await _client.PostAsync($"api/car/{car.Id}/repair", repairModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);           
        }
        [Fact]
        public async Task DeleteAllRepairs_ForExistingCarForCarOwner_ReturnsNoContent()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 1
            };

            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/repair");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteAllRepairs_ForExistingCarForNonCarOwner_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 2
            };

            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/repair");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task DeleteAllRepairs_ForNonExistingCarForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 2
            };

            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);

            var response = await _client.DeleteAsync($"api/car/{999}/repair");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task DeleteRepair_ForExistingCarAndRepairForCarOwner_ReturnsNoContent()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 1
            };

            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/repair/{repair.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteRepair_ForExistingCarForNonExistingRepairForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 1
            };          

            var response = await _client.DeleteAsync($"api/car/{car.Id}/repair/{999}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task DeleteRepair_ForExistingCarAndRepairForNonCarOwner_Returnsorbidden()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 2
            };

            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/repair/{repair.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task UpdateRepair_ForExistingCarAndRepairForCarOwner_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 1
            };
            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);
            var response = await _client.PutAsync($"api/car/{car.Id}/repair/{repair.Id}", new UpdateRepairDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateRepair_ForExistingCarForNonExistingRepairForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 1
            };
            
            var response = await _client.PutAsync($"api/car/{car.Id}/repair/{999}", new UpdateRepairDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task UpdateRepair_ForExistingCarAndRepairForNonCarOwner_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 2
            };
            var repair = new Repair()
            {
                CarId = car.Id
            };

            AssignRepairToCar(car, repair);
            var response = await _client.PutAsync($"api/car/{car.Id}/repair/{repair.Id}", new UpdateRepairDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task UpdateRepair_ForExistingCarForInvalidRepairForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "RepairTest",
                ModelName = "RepairTest",
                CreatedByUserId = 2
            };
            var repair = new Repair()
            {
                CarId = 999
            };

            seedCar(car);

            SeedRepair(repair);
            var response = await _client.PutAsync($"api/car/{car.Id}/repair/{repair.Id}", new UpdateRepairDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
