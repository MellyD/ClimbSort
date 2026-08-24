using FontRecommender.Core.Interfaces;
using FontRecommender.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FontRecommender.Controllers
{
    /// <summary>
    /// This controller handles all Grade endpoints.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class GradeController: BaseController
    {
        private readonly IFontService _fontService;
        private readonly ILogger _logger;
        public GradeController(IFontService fontService,
            ILogger logger) 
        {
            _fontService = fontService;
            _logger = logger;
        }

        /// <summary>
        /// This endpoint returns view models of all of the Grading Systems registered into the system.
        /// </summary>
        /// <returns>IEnumerable of all the Grading Systems view models.</returns>
        /// <response code="200">All Grading Systems returned in view models.</response>
        /// <response code="404">No Grading Systems found.</response>
        [HttpGet]
        public IActionResult GetGradingSystems()
        {
            try
            {
                IEnumerable<GradingSystemModel> models = _fontService.GetGradingSystems();

                return Ok(models);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error("Something went wront fetching grading systems. Ex: {ex}", ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wront fetching grading systems. Ex: {ex}", ex.Message);
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint returns a view model of the Grading System matching the unique identifier provided.
        /// </summary>
        /// <param name="gradingSystemId">Unique identifier of a Grading System.</param>
        /// <returns>View model of a Grading System.</returns>
        /// <response code="200">View model of the Grading System matching the unique identifier provided.</response>
        /// <response code="404">No Grading System found.</response>
        [HttpGet("/api/[controller]/{gradingSystemId}")]
        public async Task<IActionResult> GetGradesForSystem([FromRoute] int gradingSystemId)
        {
            try
            {
                IEnumerable<GradeModel> models = await _fontService.GetGradesForSystem(gradingSystemId);

                return Ok(models);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error("Something went wront fetching grades from grading system. Ex: {ex}", ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wront fetching grades from grading system. Ex: {ex}", ex.Message);
                return Problem();
            }
        }
    }
}
