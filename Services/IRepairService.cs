using CarAPI.Models;

namespace CarAPI.Services
{
    public interface IRepairService
    {
        int Create(int carId, NewRepairDto dto);
        void DeleteAll(int carId);
        void DeleteById(int carId, int repairId);
        IEnumerable<RepairDto> GetAll(int carId);
        RepairDto GetById(int carId, int repairId);
        void UpdateRepair(int carId, int repairId, UpdateRepairDto dto);
    }
}