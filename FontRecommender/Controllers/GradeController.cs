using FontRecommender.Core.Interfaces;
using FontRecommender.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FontRecommender.Controllers
{
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
