using FontRecommender.Core.Interfaces;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using FontRecommender.Core.ViewModels.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FontRecommender.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CragController : BaseController
    {
        private readonly IFontService _fontService;
        private readonly ILogger _logger;
        public CragController(IFontService fontService, ILogger logger)
        {
            _fontService = fontService;
            _logger = logger;
        }

        [HttpPost("/api/[controller]/GetAll")]
        public async Task<IActionResult> GetCrags([FromBody] CragFilter filter)
        {
            try
            {
                IEnumerable<CragSimpleModel> crags = await _fontService.GetCrags(filter);
                return Ok(crags);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting crags.");
                return Problem();
            }
        }

        [HttpPost("api/[controller]/Keyset")]
        public async Task<IActionResult> GetCragsKeysetPaginated([FromBody] KeysetCragFilter filter)
        {
            try
            {
                KeysetPaginateView<CragSimpleModel> crags = await _fontService.GetCragsKeysetPaginated<CragSimpleModel>(filter);
                return Ok(crags);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting crags.");
                return Problem();
            }
        }

        [HttpGet("/api/[controller]/{cragId}")]
        public async Task<IActionResult> GetCrag(Guid cragId)
        {
            try
            {
                CragModel crag = await _fontService.GetCrag(cragId);
                return Ok(crag);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Crag not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting crag.");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCrag([FromBody] CragModel cragModel)
        {
            try
            {
                Guid cragId = await _fontService.CreateCrag(cragModel);
                return Ok(cragId);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Crag not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating crag.");
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCrag([FromBody] CragModel cragModel)
        {
            try
            {
                Guid cragId = await _fontService.UpdateCrag(cragModel);
                return Ok(cragId);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Crag not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating crag.");
                return Problem();
            }
        }

        [HttpDelete("/api/[controller]/{cragId}")]
        public async Task<IActionResult> DeleteCrag(Guid cragId)
        {
            try
            {
                await _fontService.DeleteCrag(cragId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while deleting crag.");
                return Problem();
            }
        }
    }
}
