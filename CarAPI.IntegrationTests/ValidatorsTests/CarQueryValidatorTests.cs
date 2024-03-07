using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Models.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CarAPI.IntegrationTests.ValidatorsTests
{
    public class CarQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<Query>()
        {
            new Query()
            {
                PageNumber = 1,
                PageSize = 5
            },
            new Query()
            {
                PageNumber = 2,
                PageSize = 10
            },
             new Query()
            {
                PageNumber = 3,
                PageSize = 15
            },
            new Query()
            {
                PageNumber = 3,
                PageSize = 15,
                SortBy = nameof(Car.BrandName)
            },

            new Query()
            {
                PageNumber = 1,
                PageSize = 5,
                SortBy = nameof(Car.Mileage)
            },
        };

            return list.Select(e => new object[] { e });          
        }
        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<Query>()
        {
            new Query()
            {
                PageNumber = 0,
                PageSize = 5
            },
            new Query()
            {
                PageNumber = 2,
                PageSize = 11
            },
             new Query()
            {
                PageNumber = 1,
                PageSize = 5,
                SortBy = nameof(Car.BodyType)
            },          
        };

            return list.Select(e => new object[] { e });
        }
        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForValidModel_ReturnsSuccess(Query model)
        {
            var validator = new CarQueryValidator();
           
            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
            
        }
        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForInvalidModel_ReturnsFailure(Query model)
        {
            var validator = new CarQueryValidator();

            var result = validator.TestValidate(model);

            result.ShouldHaveAnyValidationError();

        }
    }
}
