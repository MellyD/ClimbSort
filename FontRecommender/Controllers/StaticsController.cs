using FontRecommender.Core.Interfaces;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static FontRecommender.Core.Enums;

namespace FontRecommender.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StaticsController: BaseController
    {
        private readonly IFontService _fontService;
        private readonly ILogger _logger;
        public StaticsController(IFontService fontService,
            ILogger logger)
        {
            _fontService = fontService;
            _logger = logger;
        }

        /// <summary>
        /// This endpoint returns view models of all Wall Types registered into the system.
        /// </summary>
        /// <returns>View models of Wall Types</returns>
        /// <response code="200">All Wall Types returned in view models.</response>
        /// <response code="404">No WallTypes found.</response>
        [HttpGet("/api/[controller]/[action]")]
        public IActionResult GetWallTypes()
        {
            try
            {
                IEnumerable<WallTypeModel> models = _fontService.GetWallTypes();

                return Ok(models);
            }
            catch(KeyNotFoundException ex)
            {
                _logger.Error("Something went wrong fetching wall types. Ex: {ex}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong fetching wall types. Ex: {ex}", ex.Message);
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves view models of all Tags in the system for either Climbs or Crags.
        /// </summary>
        /// <param name="forClimbs">Boolean indicating whether Climbs' or Crags' Tags are being retrieved.</param>
        /// <returns>View models of all Tags for either Climbs or Crags</returns>
        /// <response code="200">All Tags for either Climbs or Crags returned in view models.</response>
        /// <response code="404">No Tags found.</response>
        [HttpGet("/api/[controller]/[action]")]
        public async Task<IActionResult> GetTags([FromQuery]bool forClimbs)
        {
            try
            {
                IEnumerable<TagModel> models = await _fontService.GetTags(forClimbs);

                return Ok(models);
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong fetching wall types. Ex: {ex}", ex.Message);
                return Problem();
            }
        }
    }
}
