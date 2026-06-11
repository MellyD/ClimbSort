using FontRecommender.Core.Interfaces;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FontRecommender.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ClimbController: BaseController
    {
        private readonly IFontService _fontService;
        private readonly ILogger _logger;
        public ClimbController(IFontService fontService, ILogger logger)
        {
            _fontService = fontService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetClimbs([FromQuery] ClimbFilter filter)
        {
            try
            {
                IEnumerable<ClimbSimpleModel> climbs = await _fontService.GetClimbs(filter);
                return Ok(climbs);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        [HttpPost("/api/[controller]/AdvancedFilter")]
        public async Task<IActionResult> AdvancedGetClimbs([FromBody] AdvancedClimbFilter filter)
        {
            try
            {
                List<ClimbSimpleModel> climbs = await _fontService.AdvancedGetClimbs(filter);
                return Ok(climbs);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        [HttpGet("/api/[controller]/{climbId}")]
        public async Task<IActionResult> GetClimb(Guid climbId)
        {
            try
            {
                ClimbModel climb = await _fontService.GetClimb(climbId);
                return Ok(climb);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climb not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climb.");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClimb([FromBody] ClimbModel climbModel)
        {
            try
            {
                Guid climbId = await _fontService.CreateClimb(climbModel);
                return Ok(climbId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating climb.");
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClimb([FromBody] ClimbModel climbModel)
        {
            try
            {
                Guid climbId = await _fontService.UpdateClimb(climbModel);
                return Ok(climbId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating climb.");
                return Problem();
            }
        }

        [HttpDelete("/api/[controller]/{climbId}")]
        public async Task<IActionResult> DeleteClimb(Guid climbId)
        {
            try
            {
                await _fontService.DeleteClimb(climbId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while deleting climb.");
                return Problem();
            }
        }
    }
}
