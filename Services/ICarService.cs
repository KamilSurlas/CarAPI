using CarAPI.Models;

namespace CarAPI.Services
{
    public interface ICarService
    {
        int Create(NewCarDto dto);
        IEnumerable<CarDto> GetAll();
        CarDto GetById(int id);
    }
}