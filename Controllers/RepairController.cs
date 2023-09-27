using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/car/{carId}/repair")]
    [ApiController]
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
    }
}
