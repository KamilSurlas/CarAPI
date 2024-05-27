using CarAPI.Constants;
using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/car/{carId}/technicalReview")]
    [ApiController]
    [Authorize(Roles = UserRoles.User + "," + UserRoles.Mechanic + "," + UserRoles.Admin)]
    public class TechnicalReviewController : ControllerBase
    {
        private readonly ITechnicalReviewService _technicalReviewService;
        public TechnicalReviewController(ITechnicalReviewService technicalReviewService)
        {
            _technicalReviewService = technicalReviewService;
        }
        [HttpPost]
        public ActionResult AddTechnicalReview([FromRoute] int carId, [FromBody] NewTechnicalReviewDto dto)
        {
            var newTechnicalReviewId = _technicalReviewService.Create(carId, dto);

            return Created($"/api/car/{carId}/technicalReview/{newTechnicalReviewId}", null);
        }
        [HttpGet("{technicalReviewId}")]
        public ActionResult<TechnicalReviewDto> GetById([FromRoute] int carId, [FromRoute] int technicalReviewId) 
        {
          TechnicalReviewDto technicalReview = _technicalReviewService.GetById(carId, technicalReviewId);
            return Ok(technicalReview);
        }
        [HttpGet]
        public ActionResult<List<TechnicalReviewDto>> GetAll([FromRoute] int carId)
        {
            var technicalReviews = _technicalReviewService.GetAll(carId);
            return Ok(technicalReviews);
        }
        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int carId)
        {
            _technicalReviewService.DeleteAll(carId);

            return NoContent();
        
        }
        [HttpDelete("{technicalReviewId}")]
        public ActionResult DeleteById([FromRoute] int carId,[FromRoute] int technicalReviewId)
        {
            _technicalReviewService.DeleteById(carId, technicalReviewId);
            return NoContent();
        }
        [HttpPut("{technicalReviewId}")]
        public ActionResult Update([FromRoute] int carId, [FromRoute] int technicalReviewId, [FromBody] UpdateTechnicalReviewDto dto)
        {
            _technicalReviewService.UpdateTechnicalReview(carId, technicalReviewId, dto);

            return Ok();
        }
    }
}
