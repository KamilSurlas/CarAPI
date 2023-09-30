using CarAPI.Models;

namespace CarAPI.Services
{
    public interface IRepairService
    {
        int Create(int carId, NewRepairDto dto);
        IEnumerable<RepairDto> GetAll(int carId);
        RepairDto GetById(int carId, int repairId);
    }
}