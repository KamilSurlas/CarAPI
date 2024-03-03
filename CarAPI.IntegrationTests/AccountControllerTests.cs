using CarAPI.Entities;
using CarAPI.Exceptions;
using CarAPI.IntegrationTests.Helpers;
using CarAPI.Models;
using CarAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Moq;
using NLog.Config;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Xunit;

namespace CarAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private Mock<IAccountService> _accountServiceMock = new();
        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CarDbContext>));

                    services.Remove(dbContextOptions);

                   services.AddSingleton<IAccountService>(_accountServiceMock.Object);

                    services
                     .AddDbContext<CarDbContext>(options => options.UseInMemoryDatabase("CarDb", db => db.EnableNullChecks(false)));

                });
            });
                _client = _factory.CreateClient();
        }
        
        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            var registerUser = new RegisterUserDto()
            {
                Email = "mailTest@test.com",
                Password = "passwordTest123",
                ConfirmPassword = "passwordTest123",
                FirstName = "TestName",
                LastName = "TestLastName",
                DateOfBirth = DateTime.Now
            };

            var response = await _client.PostAsync("/api/account/register", registerUser.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            var registerUser = new RegisterUserDto()
            {
                Email = "mailTest@test.com",
                Password = "passwordTest123",
                ConfirmPassword = "badConfirmPassword",
                FirstName = "TestName",
                LastName = "TestLastName",
                DateOfBirth = DateTime.Now
            };

            var response = await _client.PostAsync("/api/account/register", registerUser.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            _accountServiceMock.Setup(e => e.GetJwtToken(It.IsAny<LoginDto>())).Returns("jwt");
                

            var loginDto = new LoginDto()
            {
                Email = "loginTest@test.com",
                Password = "loginPasswordTest"
            };


            var response = await _client.PostAsync("/api/account/login", loginDto.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Login_ForBadPasswordOrEmail_ReturnsBadRequest()
        {
            _accountServiceMock.Setup(e => e.GetJwtToken(It.IsAny<LoginDto>())).Throws(new BadEmailOrPassword(" "));
 
            var loginDto = new LoginDto()
            {
                Email = "LoginTest@test.com",
                Password = "ThatIsBadPassword"
            };
            
           

            var response = await _client.PostAsync("/api/account/login", loginDto.ToJsonHttpContent());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
          
        }
    }
}
