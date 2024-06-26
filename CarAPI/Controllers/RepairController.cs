﻿using CarAPI.Constants;
using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/car/{carId}/repair")]
    [ApiController]
    [Authorize(Roles = UserRoles.User + "," + UserRoles.Mechanic + "," + UserRoles.Admin)]
    public class RepairController: ControllerBase
    {
        private readonly IRepairService _repairService;
        public RepairController(IRepairService repairService)
        {
            _repairService = repairService;
        }

        [HttpPost]
        public ActionResult AddRepair([FromRoute] int carId, [FromBody] NewRepairDto dto)
        {
            var newRepairId = _repairService.Create(carId, dto);

            return Created($"/api/car/{carId}/repair/{newRepairId}", null);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<RepairDto>> GetAll([FromRoute] int carId)
        {
            var repairs = _repairService.GetAll(carId);
            return Ok(repairs);
        }
        [HttpGet("{repairId}")]
        [AllowAnonymous]
        public ActionResult<RepairDto> GetById([FromRoute] int carId, [FromRoute] int repairId)
        {
            RepairDto repair = _repairService.GetById(carId, repairId);
            return Ok(repair);
        }
        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int carId) 
        {
            _repairService.DeleteAll(carId);

            return NoContent();
        }
        [HttpDelete("{repairId}")]
        public ActionResult DeleteById([FromRoute] int carId, [FromRoute] int repairId)
        {
            _repairService.DeleteById(carId, repairId);
            return NoContent();
        }
        [HttpPut("{repairId}")]
        public ActionResult Update([FromRoute] int carId, [FromRoute] int repairId, [FromBody] UpdateRepairDto dto)
        {
            _repairService.UpdateRepair(carId, repairId, dto);

            return Ok();
        }
    }
}
