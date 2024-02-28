using AutoMapper;
using CarAPI.Entities;
using CarAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace CarAPI.Mapper
{
    public class RegisterUserConverter : ITypeConverter<RegisterUserDto, User>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        public RegisterUserConverter(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public User Convert(RegisterUserDto source, User destination, ResolutionContext context)
        {
            destination = new User()
            {
                Email = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth,
                RoleId = source.RoleId
            };

            destination.HashedPassword = _passwordHasher.HashPassword(destination,source.Password);
            return destination;
        }
    }
}
