using CarAPI.Entities;
using CarAPI.Enums;
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
    public class TechnicalReviewControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        public TechnicalReviewControllerTests(WebApplicationFactory<Program> factory)
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
        private void AssignTechnicalReviewToCar(Car car, TechnicalReview technicalReview)
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();
            var dbContext = createdScope?.ServiceProvider.GetService<CarDbContext>();

            dbContext?.Cars.Add(car);
            car.TechnicalReviews = new List<TechnicalReview>
            {
                technicalReview
            };



            dbContext?.TechnicalReviews.Add(technicalReview);
            dbContext?.SaveChanges();
        }
        private void SeedTechnicalReview(TechnicalReview technicalReview)
        {
            var scope = _factory.Services.GetService<IServiceScopeFactory>();
            using var createdScope = scope?.CreateScope();
            var dbContext = createdScope?.ServiceProvider.GetService<CarDbContext>();



            dbContext?.TechnicalReviews.Add(technicalReview);
            dbContext?.SaveChanges();
        }

        [Fact]
        public async Task GetAllTechnicalReviews_ForExistingCar_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
            };

            SeedCar(car);

            var response = await _client.GetAsync($"api/car/{car.Id}/technicalReview");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(999)]
        [InlineData(null)]
        public async Task GetAllTechnicalReviews_ForNonExistingCar_ReturnsNotFound(int id)
        {
            var response = await _client.GetAsync($"api/car/{id}/technicalReview");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task GetTechnicalReviewById_ForCorrectData_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
            };

            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);

            var response = await _client.GetAsync($"api/car/{car.Id}/technicalReview/{technicalReview.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
        [Theory]
        [InlineData(9999, 9999)]
        [InlineData(1, 9999)]
        [InlineData(99999, 1)]
        public async Task GetTechnicalReviewById_ForIncorrectData_ReturnsNotFound(int carId, int technicalReviewId)
        {
            var response = await _client.GetAsync($"api/car/{carId}/technicalReview/{technicalReviewId}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public async Task CreateTechnicalReview_ForExistingCarWithValidModel_ReturnsCreated()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };

            SeedCar(car);

            var technicalReviewModel = new NewTechnicalReviewDto()
            {
                Description = "TechnicalReviewTest",
                TechnicalReviewDate = DateTime.Now,
                TechnicalReviewResult = TechnicalReviewResult.Positive
            };

            var response = await _client.PostAsync($"api/car/{car.Id}/technicalReview", technicalReviewModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }
        [Fact]
        public async Task CreateTechnicalReview_ForNonExistingCarWithValidModel_ReturnsNotFound()
        {

            var technicalReviewModel = new NewTechnicalReviewDto()
            {
                Description = "TechnicalReviewTest",
                TechnicalReviewDate = DateTime.Now,
                TechnicalReviewResult = TechnicalReviewResult.Positive
            };
            var response = await _client.PostAsync($"api/car/{999}/technicalReview", technicalReviewModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task CreateTechnicalReview_ForNonCarOwnerWithValidModel_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 2
            };

            SeedCar(car);
            var technicalReviewModel = new NewTechnicalReviewDto()
            {
                Description = "TechnicalReviewTest",
                TechnicalReviewDate = DateTime.Now,
                TechnicalReviewResult = TechnicalReviewResult.Positive
            };
            var response = await _client.PostAsync($"api/car/{car.Id}/technicalReview", technicalReviewModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task CreateTechnicalReview_ForExistingCarWithInvalidModel_ReturnsBadRequest()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };

            SeedCar(car);

            var technicalReviewModel = new NewTechnicalReviewDto()
            {
               Description = "Test description for technical review"
            };

            var response = await _client.PostAsync($"api/car/{car.Id}/technicalReview", technicalReviewModel.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task DeleteAllTechnicalReviews_ForExistingCarForCarOwner_ReturnsNoContent()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };

            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/technicalReview");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteAllTechnicalReviews_ForExistingCarForNonCarOwner_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 2
            };

            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/technicalReview");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task DeleteAllTechnicalReviews_ForNonExistingCarForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 2
            };

            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);

            var response = await _client.DeleteAsync($"api/car/{999}/technicalReview");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task DeleteTechnicalReview_ForExistingCarAndTechnicalReviewForCarOwner_ReturnsNoContent()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };

            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/technicalReview/{technicalReview.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteTechnicalReview_ForExistingCarForNonExistingTechnicalReviewForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };

            SeedCar(car);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/technicalReview/{999}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task DeleteTechnicalReview_ForExistingCarAndTechnicalReviewForNonCarOwner_Returnsorbidden()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 2
            };

            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);

            var response = await _client.DeleteAsync($"api/car/{car.Id}/technicalReview/{technicalReview.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task UpdateTechnicalReview_ForExistingCarAndTechnicalReviewForCarOwner_ReturnsOK()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };
            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);
            var response = await _client.PutAsync($"api/car/{car.Id}/technicalReview/{technicalReview.Id}", new UpdateTechnicalReviewDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateTechnicalReview_ForExistingCarForNonExistingTechnicalReviewForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 1
            };

            var response = await _client.PutAsync($"api/car/{car.Id}/technicalReview/{999}", new UpdateTechnicalReviewDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task UpdateTechnicalReview_ForExistingCarAndTechnicalReviewForNonCarOwner_ReturnsForbidden()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 2
            };
            var technicalReview = new TechnicalReview()
            {
                CarId = car.Id
            };

            AssignTechnicalReviewToCar(car, technicalReview);
            var response = await _client.PutAsync($"api/car/{car.Id}/technicalReview/{technicalReview.Id}", new UpdateTechnicalReviewDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task UpdateTechnicalReview_ForExistingCarForInvalidTechnicalReviewForCarOwner_ReturnsNotFound()
        {
            var car = new Car()
            {
                BrandName = "TechnicalReviewTest",
                ModelName = "TechnicalReviewTest",
                CreatedByUserId = 2
            };
            var technicalReview = new TechnicalReview()
            {
                CarId = 999
            };

            SeedCar(car);

            SeedTechnicalReview(technicalReview);
            var response = await _client.PutAsync($"api/car/{car.Id}/technicalReview/{technicalReview.Id}", new UpdateTechnicalReviewDto().ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
