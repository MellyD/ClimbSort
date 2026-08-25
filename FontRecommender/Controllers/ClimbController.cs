using ClimbSort.Core.Interfaces;
using ClimbSort.Core.ViewModels;
using ClimbSort.Core.ViewModels.Filters;
using ClimbSort.Core.ViewModels.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ClimbSort.Controllers
{
    /// <summary>
    /// This controller handles all Climb endpoints.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class ClimbController: BaseController
    {
        //Class is kept simple, managing all logic in the service class.
        private readonly IClimbSortService _climbSortService;
        private readonly ILogger _logger;

        /// <summary>
        /// This controller requires the service interface and logger only, keeping it low complexity.
        /// </summary>
        /// <param name="fontService"></param>
        /// <param name="logger"></param>
        public ClimbController(IClimbSortService fontService, ILogger logger)
        {
            _climbSortService = fontService;
            _logger = logger;
        }

        /// <summary>
        /// This endpoint retrieves a list of climbs based on the provided filter criteria. The filter can include parameters such as wall type IDs, tags, and other climb attributes. 
        /// The response will contain a collection of ClimbSimpleModel objects that match the specified filter.
        /// </summary>
        /// <param name="filter">
        /// - name: string
        /// - minGradeScaleOrder: int
        /// - maxGradeScaleOrder: int
        /// - cragId: guid
        /// - circuitId: guid
        /// - minRating: decimal
        /// - maxRating: decimal
        /// - minPopularity: int
        /// - maxPopularity: int
        /// - sitStart: bool
        /// </param>
        /// <returns> List of climb view models, climbs returned based on filter provided.</returns>
        /// <response code="200">Climbs matching the filter were found.</response>
        /// <response code="404">No climbs exist that match the given filter.</response>
        [HttpGet]
        public async Task<IActionResult> GetClimbs([FromQuery] ClimbFilter filter)
        {
            try
            {
                IEnumerable<ClimbSimpleModel> climbs = await _climbSortService.GetClimbs(filter);
                return Ok(climbs);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climbs not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves a list of climbs based on the provided advanced filter criteria. 
        /// The advanced filter allows for more complex queries, including multiple filter components that can specify various attributes of climbs from various crags.
        /// </summary>
        /// <param name="filter">
        /// Takes a list of AdvancedClimbFilterComponent objects. This inherits from the ClimbFilter but has the addition of:
        /// - wallTypeId: int
        /// - tags: list of ints
        /// Also separates out name for easier searching.
        /// </param>
        /// <returns> List of climb view models, climbs returned based on filter provided.</returns>
        /// <response code="200">Climbs matching the filter were found.</response>
        /// <response code="404">No climbs exist that match the given filter.</response>
        [HttpPost("/api/[controller]/AdvancedFilter")]
        public async Task<IActionResult> AdvancedGetClimbs([FromBody] AdvancedClimbFilter filter)
        {
            try
            {
                IEnumerable<ClimbSimpleModel> climbs = await _climbSortService.GetClimbs(filter);
                return Ok(climbs);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climbs not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves a paginated list of climbs based on the provided offset filter criteria. Input contains filter along with pagination details like page number and page size.
        /// </summary>
        /// <param name="filter">
        /// Regular filter with added:
        /// - pageSize: int
        /// - pageNumber: int
        /// </param>
        /// <returns> List of climb view models, climbs returned based on filter provided.</returns>
        /// <response code="200">Climbs matching the filter were found.</response>
        /// <response code="404">No climbs exist that match the given filter.</response>
        [HttpGet("/api/[controller]/Offset")]
        public async Task<IActionResult> GetClimbsOffsetPaginated([FromQuery] OffsetClimbFilter filter)
        {
            try
            {
                OffsetPaginateView<ClimbSimpleModel> climbs = await _climbSortService.GetClimbsOffsetPaginated<ClimbSimpleModel>(filter);
                return Ok(climbs);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climbs not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves a paginated list of climbs based on the provided keyset filter criteria. Input contains filter along with pagination details like page size and last item ID for keyset pagination.
        /// </summary>
        /// <param name="filter">
        /// Regular filter with added:
        /// - pageSize: int
        /// - lastitem: guid
        /// </param>
        /// <returns> List of climb view models, climbs returned based on filter provided.</returns>
        /// <response code="200">Climbs matching the filter were found.</response>
        /// <response code="404">No climbs exist that match the given filter.</response>
        [HttpGet("/api/[controller]/Keyset")]
        public async Task<IActionResult> GetClimbsKeysetPaginated([FromQuery] KeysetClimbFilter filter)
        {
            try
            {
                KeysetPaginateView<ClimbSimpleModel> climbs = await _climbSortService.GetAllClimbsKeysetPaginated<ClimbSimpleModel>(filter);
                return Ok(climbs);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climbs not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves a paginated list of climbs based on the provided advanced offset filter criteria. Input contains advanced filter along with pagination details like page number and page size.
        /// </summary>
        /// <param name="filter">
        /// Advanced filter with added:
        /// - pageSize: int
        /// - pageNumber: int
        /// </param>
        /// <returns> List of climb view models, climbs returned based on filter provided.</returns>
        /// <response code="200">Climbs matching the filter were found.</response>
        /// <response code="404">No climbs exist that match the given filter.</response>
        [HttpPost("/api/[controller]/Offset/AdvancedFilter")]
        public async Task<IActionResult> GetAdvancedClimbsOffsetPaginated([FromBody] OffsetAdvancedClimbFilter filter)
        {
            try
            {
                OffsetPaginateView<ClimbSimpleModel> climbs = await _climbSortService.GetClimbsOffsetPaginated<ClimbSimpleModel>(filter);
                return Ok(climbs);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climbs not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves a paginated list of climbs based on the provided advanced keyset filter criteria. Input contains advanced filter along with pagination details like page size and last item ID for keyset pagination.
        /// </summary>
        /// <param name="filter">
        /// Advanced filter with added:
        /// - pageSize: int
        /// - lastitem: guid
        /// </param>
        /// <returns> List of climb view models, climbs returned based on filter provided.</returns>
        /// <response code="200">Climbs matching the filter were found.</response>
        /// <response code="404">No climbs exist that match the given filter.</response>
        [HttpPost("/api/[controller]/Keyset/AdvancedFilter")]
        public async Task<IActionResult> GetAdvancedClimbsKeysetPaginated([FromBody] KeysetAdvancedClimbFilter filter)
        {
            try
            {
                KeysetPaginateView<ClimbSimpleModel> climbs = await _climbSortService.GetAllClimbsKeysetPaginated<ClimbSimpleModel>(filter);
                return Ok(climbs);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Climbs not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting climbs.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint retrieves a specific climb based on the provided climb ID. The response will contain a ClimbModel object that represents the details of the requested climb.
        /// </summary>
        /// <param name="climbId"> Climb's unique identifier. </param>
        /// <returns> Respective climb view model. </returns>
        /// <response code="200">The climb was found.</response>
        /// <response code="404">No climb exists with the specified ID.</response>
        [HttpGet("/api/[controller]/{climbId}")]
        public async Task<IActionResult> GetClimb(Guid climbId)
        {
            try
            {
                ClimbModel climb = await _climbSortService.GetClimb(climbId);
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

        /// <summary>
        /// This endpoint creates a new climb based on the provided ClimbModel. The request body should contain the details of the climb to be created. 
        /// Upon successful creation, the endpoint returns the ID of the newly created climb.
        /// </summary>
        /// <param name="climbModel">
        /// - wallTypeId: int
        /// - climbId: guid (nullable)
        /// - name: string
        /// - searchName: string (nullable)
        /// - popularity: int (nullable)
        /// - gradeLabel: string (nullable)
        /// - gradeScaleOrder: int (nullable)
        /// - cragName: string (nullable)
        /// - rating: decimal (nullable)
        /// - link: string (nullable)
        /// - tags: list of TagModel (nullable)
        /// - cragId: guid (nullable)
        /// - circuitId: guid (nullable)
        /// - circuitColour: string (nullable)
        /// - description: string (nullable)
        /// - circuitNumber: int (nullable)
        /// - sitStart: bool (nullable)
        /// - topoImageReference: string (nullable)
        /// - cragCoordinates: list of CoordinatesModel (nullable)
        /// - topoCoordinates: list of CoordinatesModel (nullable)
        /// - coordinates: list of CoordinatesModel (nullable)
        /// Coordinates Model:
        /// - latitude: decimal
        /// - longitude: decimal
        /// - coordinateType: int/string (enum)
        /// Tag Model:
        /// - tagName: string
        /// - tagId : int
        /// </param>
        /// <returns> Guid id of the climb created.</returns>
        /// <response code="200">The climb was created successfully, returning the unique identifier.</response>
        /// <response code="400">Issue was found with the provided Climb information that prevented creation.</response>
        /// <response code="404">No climb exists with the specified ID.</response>
        [HttpPost]
        public async Task<IActionResult> CreateClimb([FromBody] ClimbModel climbModel)
        {
            try
            {
                Guid climbId = await _climbSortService.CreateClimb(climbModel);
                return Ok(climbId);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Error occurred while creating climb.");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Error occurred while creating climb.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating climb.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint updates an existing climb based on the provided ClimbModel. The request body should contain the updated details of the climb, including its ID.
        /// </summary>
        /// <param name="climbModel">
        /// - wallTypeId: int
        /// - climbId: guid (nullable)
        /// - name: string
        /// - searchName: string (nullable)
        /// - popularity: int (nullable)
        /// - gradeLabel: string (nullable)
        /// - gradeScaleOrder: int (nullable)
        /// - cragName: string (nullable)
        /// - rating: decimal (nullable)
        /// - link: string (nullable)
        /// - tags: list of TagModel (nullable)
        /// - cragId: guid (nullable)
        /// - circuitId: guid (nullable)
        /// - circuitColour: string (nullable)
        /// - description: string (nullable)
        /// - circuitNumber: int (nullable)
        /// - sitStart: bool (nullable)
        /// - topoImageReference: string (nullable)
        /// - cragCoordinates: list of CoordinatesModel (nullable)
        /// - topoCoordinates: list of CoordinatesModel (nullable)
        /// - coordinates: list of CoordinatesModel (nullable)
        /// Coordinates Model:
        /// - latitude: decimal
        /// - longitude: decimal
        /// - coordinateType: int/string (enum)
        /// Tag Model:
        /// - tagName: string
        /// - tagId : int
        /// </param>
        /// <returns> Guid id of the climb created.</returns>
        /// <response code="200">The climb was updated successfully, returning the unique identifier.</response>
        /// <response code="400">Issue was found with the provided Climb information that prevented the update.</response>
        /// <response code="404">No climb exists with the specified ID.</response>
        [HttpPut]
        public async Task<IActionResult> UpdateClimb([FromBody] ClimbModel climbModel)
        {
            try
            {
                Guid climbId = await _climbSortService.UpdateClimb(climbModel);
                return Ok(climbId);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Error occurred while updating climb.");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Error occurred while updating climb.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating climb.");
                return Problem();
            }
        }

        /// <summary>
        /// This endpoint deletes an existing climb based on the provided climb ID. The climb will be removed from the system, and a successful response indicates that the deletion was completed.
        /// </summary>
        /// <param name="climbId"> Unique identifier of the Climb to be deleted.</param>
        /// <returns> Void return. </returns>
        /// <response code="200">The climb was deleted successfully.</response>
        /// <response code="404">No climb exists with the specified ID.</response>
        [HttpDelete("/api/[controller]/{climbId}")]
        public async Task<IActionResult> DeleteClimb(Guid climbId)
        {
            try
            {
                await _climbSortService.DeleteClimb(climbId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Error(ex, "Error occurred while deleting climb.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while deleting climb.");
                return Problem();
            }
        }
    }
}
