﻿using AutoMapper;
using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarAPI.Controllers
{
    [Route("api/car")]
    [ApiController]
    [Authorize]
    public class CarController: ControllerBase
    {
        private readonly ICarService _carService;
       

        public CarController(ICarService carService)
        {
           _carService = carService;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<CarDto>> GetAll([FromQuery] Query query)
        {
            var cars = _carService.GetAll(query);
                
            return Ok(cars);
        }
        [HttpGet("{carId}")]
        [AllowAnonymous]
        public ActionResult<CarDto> GetById([FromRoute] int carId) 
        {
            var car = _carService.GetById(carId);
         
            return Ok(car);
        }
        [HttpGet("byRegistrationNumber/{registrationNumber}")]
        [AllowAnonymous]
        public ActionResult<CarDto> GetByRegistrationNumber([FromRoute] string registrationNumber)
        {
            var car = _carService.GetByRegistrationNumber(registrationNumber);

            return Ok(car);
        }
        [HttpPost]
        public ActionResult AddCar([FromBody] NewCarDto dto)
        {           
            var id = _carService.Create(dto);
            return Created($"/api/car/{id}", null);
        }
        [HttpDelete("{carId}")]
        public ActionResult Delete([FromRoute] int carId)
        {
           _carService.Delete(carId);
           
            return NoContent();
        }
        [HttpPut("{carId}")]
        public ActionResult Update([FromBody] UpdateCarDto dto, [FromRoute] int carId)
        {         
            _carService.Update(carId, dto);

            return Ok();
        }
    }
}
