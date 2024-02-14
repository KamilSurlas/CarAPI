using CarAPI.Models;
using System.Security.Claims;

namespace CarAPI.Services
{
    public interface ICarService
    {
        int Create(NewCarDto dto);
        void Delete(int carId);
        IEnumerable<CarDto> GetAll(string searchPhrase);
        CarDto GetById(int id);
        CarDto GetByRegistrationNumber(string registrationNumber);
        public void Update(int carId, UpdateCarDto dto);
    }
}