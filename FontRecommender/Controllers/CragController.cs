using FontRecommender.Core.Interfaces;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using FontRecommender.Core.ViewModels.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FontRecommender.Controllers
{
    /// <summary>
    /// This controller handles all Crag endpoints.
    /// </summary>
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

        /// <summary>
        /// This endpoint retrieves all of the Crags that match the given filter provided.
        /// </summary>
        /// <param name="filter">
        /// - name: string (nullable)
        /// - countryCode: string (nullable)
        /// - tags: list of int (nullable)
        /// </param>
        /// <returns>IEnumerable of the Crags that match the filter provided.</returns>
        /// <response code="200">Crags matching the filter were found.</response>
        /// <response code="404">No Crags exist that match the given filter.</response>
        [HttpPost("/api/[controller]/GetAll")]
        public async Task<IActionResult> GetCrags([FromBody] CragFilter filter)
        {
            try
            {
                IEnumerable<CragSimpleModel> crags = await _fontService.GetCrags(filter);
                return Ok(crags);
            }
            catch(KeyNotFoundException ex)
            {
                _logger.Error(ex, "Error occurred while getting crags.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting crags.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves all of the Crags that match the given filter provided. Keyset paginated based on pagination info given.
        /// </summary>
        /// <param name="filter">
        /// Crag filter with additional keyset pagination information:
        /// - pageSize: int
        /// - lastItem: guid (nullable)
        /// </param>
        /// <returns>IEnumerable of the Crags that match the filter provided.</returns>
        /// <response code="200">Crags matching the filter were found.</response>
        /// <response code="404">No Crags exist that match the given filter.</response>
        [HttpPost("api/[controller]/Keyset")]
        public async Task<IActionResult> GetCragsKeysetPaginated([FromBody] KeysetCragFilter filter)
        {
            try
            {
                KeysetPaginateView<CragSimpleModel> crags = await _fontService.GetCragsKeysetPaginated<CragSimpleModel>(filter);
                return Ok(crags);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Error occurred while getting crags.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting crags.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves the Crag matching the provided unique identifier.
        /// </summary>
        /// <param name="cragId"> Unique identifier of a Crag.</param>
        /// <returns> View model of a Crag.</returns>
        /// <response code="200">Crag matching the unique identifier was found.</response>
        /// <response code="404">No Crag exists matching the given unique identifier.</response>
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

        /// <summary>
        /// This endpoint creates a new Crag based on the CragModel view model provided.
        /// Upon successful creation, the endpoint returns the ID of the newly created crag.
        /// </summary>
        /// <param name="cragModel">
        /// name: string
        /// searchName: string (nullable)
        /// countryCode: string
        /// tags: list of tag view models
        /// Tag Model:
        /// - tagName: string
        /// - tagId : int
        /// </param>
        /// <returns>Unique identifier of the created Crag.</returns>
        /// <response code="200">Crag created successfully and unique identifier returned.</response>
        /// <response code="400">Issue was found with the provided Crag information that prevented creation.</response>
        [HttpPost]
        public async Task<IActionResult> CreateCrag([FromBody] CragModel cragModel)
        {
            try
            {
                Guid cragId = await _fontService.CreateCrag(cragModel);
                return Ok(cragId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Crag not found.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating crag.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint updates an existring Crag based on the CragModel view model provided.
        /// Upon successful updating, the endpoint returns the ID of the updated crag.
        /// </summary>
        /// <param name="cragModel">
        /// name: string
        /// searchName: string (nullable)
        /// countryCode: string
        /// tags: list of tag view models
        /// Tag Model:
        /// - tagName: string
        /// - tagId : int
        /// </param>
        /// <returns>Unique identifier of the updated Crag.</returns>
        /// <response code="200">Crag updated successfully and unique identifier returned.</response>
        /// <response code="400">Issue was found with the provided Crag information that prevented updating.</response>
        /// <response code="404">No Crag exists matching the given unique identifier.</response>
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

        /// <summary>
        /// This endpoint deletes an existing Crag that matches the given unique identifier.
        /// </summary>
        /// <param name="cragId">Unique identifier of a Crag.</param>
        /// <returns>Void return.</returns>
        /// <response code="200">Crag deleted successfully.</response>
        /// <response code="404">No Crag exists matching the given unique identifier.</response>
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
