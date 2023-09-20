using CarAPI.Models;

namespace CarAPI.Services
{
    public interface ICarService
    {
        int Create(NewCarDto dto);
        void Delete(int carId);
        IEnumerable<CarDto> GetAll();
        CarDto GetById(int id);
        public void Update(int carId, UpdateCarDto dto);
    }
}