using AutoMapper;
using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Controllers
{
    [Route("api/car")]
    public class CarController: ControllerBase
    {
        private readonly ICarService _carService;
       

        public CarController(ICarService carService)
        {
           _carService = carService;
        }
        public ActionResult<IEnumerable<CarDto>> GetAll()
        {
            var cars = _carService.GetAll();
                
            return Ok(cars);
        }

        [HttpGet("{carId}")]
        public ActionResult<CarDto> GetById([FromRoute] int carId) 
        {
            var car = _carService.GetById(carId);

            if (car is null)
            {
                return NotFound();
            }

            return Ok(car);
        }
        [HttpPost]
        public ActionResult AddCar([FromBody] NewCarDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }         
            var id = _carService.Create(dto);

            return Created($"/api/car/{id}", null);
        }
        [HttpDelete("{carId}")]
        public ActionResult Delete([FromRoute] int carId)
        {
            var isDeleted = _carService.Delete(carId);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
