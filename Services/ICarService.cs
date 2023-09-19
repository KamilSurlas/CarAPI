using CarAPI.Models;

namespace CarAPI.Services
{
    public interface ICarService
    {
        int Create(NewCarDto dto);
        bool Delete(int carId);
        IEnumerable<CarDto> GetAll();
        CarDto GetById(int id);
        public bool Update(int carId, UpdateCarDto dto);
    }
}