using CarAPI.Models;

namespace CarAPI.Services
{
    public interface IAccountService
    {
        string GetJwtToken(LoginDto dto);
        void RegisterUser(RegisterUserDto dto);
    }
}