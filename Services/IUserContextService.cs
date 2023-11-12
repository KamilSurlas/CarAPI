using System.Security.Claims;

namespace CarAPI.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal? User { get; }
        int? UserId { get; }
    }
}