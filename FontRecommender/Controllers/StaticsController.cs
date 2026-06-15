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

        [HttpGet("/api/[controller]/[action]")]
        public IActionResult GetTags()
        {
            try
            {
                List<TagModel> models = GetTagModels();

                return Ok(models);
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong fetching wall types. Ex: {ex}", ex.Message);
                return Problem();
            }
        }

        private static List<TagModel> GetTagModels()
        {
            return Enum.GetValues<eTag>()
                .Select(tag => new TagModel
                {
                    TagName = Enum.GetName(tag) ?? tag.ToString(),
                    TagId = (int)tag
                })
                .ToList();
        }
    }
}
