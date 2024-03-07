using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Models.Validators;
using CarAPI.Seeder;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarAPI.IntegrationTests.ValidatorsTests
{
    public class RegisterUserDtoValidatorTests
    {
        private CarDbContext _dbContext;
        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<CarDbContext>();
            builder.UseInMemoryDatabase("TestCarDb", db => db.EnableNullChecks(false));

            _dbContext = new CarDbContext(builder.Options);

            SeedTestDb();
        }
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<RegisterUserDto>()
        {
            new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123test",
                ConfirmPassword = "password123test"
            },
            new RegisterUserDto()
            {
                 Email = "testTest@test.com",
                Password = "password123test2",
                ConfirmPassword = "password123test2"
            }
            };
            return list.Select(e => new object[] { e });

        }
        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            
           var list = new List<RegisterUserDto>()
        {
            new RegisterUserDto()
            {
                Email = "test2@test.com",
                Password = "password123test",
                ConfirmPassword = "password123test"
            },
            new RegisterUserDto()
            {
                 Email = "testTest@test.com",
                Password = "password123test2",
                ConfirmPassword = "123passwordtest2"
            }
            };
            return list.Select(e => new object[] { e });

        }
        private void SeedTestDb()
        {
            var testUsers = new List<User>()
            {
                new User()
                {
                    Email = "test2@test.com"
                },
                new User()
                {
                 Email = "test3@test.com"
                }
            };

            _dbContext.Users.AddRange(testUsers);
            _dbContext.SaveChanges();
        }
        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForValidModel_ReturnsSuccess(RegisterUserDto model)
        {       
            var validator = new RegisterUserDtoValidator(_dbContext);

            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForInvalidModel_ReturnsSuccess(RegisterUserDto model)
        {
            var validator = new RegisterUserDtoValidator(_dbContext);

            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();
        }
    }
}
