using CarAPI.Constants;
using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/car/{carId}/insurance")]
    [ApiController]
    [Authorize(Roles = UserRoles.User + "," + UserRoles.Insurer + "," + UserRoles.Admin)]
    public class InsuranceController:ControllerBase
    {
        private readonly IInsuranceService _insuranceService;
        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }       
        [HttpGet]
        public ActionResult<InsuranceDto> Get([FromRoute] int carId)
        {
            var insurance = _insuranceService.GetInsurance(carId);
            return Ok(insurance);
        }
        [HttpPut]
        public ActionResult Update([FromRoute] int carId,[FromBody] UpdateInsuranceDto dto)
        {
            _insuranceService.UpdateInsurance(carId,dto);

            return Ok();
        }
    }
}
