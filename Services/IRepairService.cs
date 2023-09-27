using CarAPI.Models;

namespace CarAPI.Services
{
    public interface IRepairService
    {
        int Create(int carId, NewRepairDto dto);
    }
}