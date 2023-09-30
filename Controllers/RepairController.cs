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
        [HttpGet]
        public ActionResult<List<RepairDto>> GetAll([FromRoute] int carId)
        {
            var repairs = _repairService.GetAll(carId);
            return Ok(repairs);
        }
        [HttpGet("{repairId}")]
        public ActionResult<TechnicalReviewDto> GetById([FromRoute] int carId, [FromRoute] int repairId)
        {
            RepairDto repair = _repairService.GetById(carId, repairId);
            return Ok(repair);
        }
    }
}
