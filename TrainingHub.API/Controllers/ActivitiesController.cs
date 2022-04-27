using Microsoft.AspNetCore.Mvc;
using TrainingHub.Infrastructure.Abstractions;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService activityService;
        public ActivitiesController(IActivityService activityService)
        {
            this.activityService = activityService;
        }

        [HttpGet("", Name = "GetPaged")]
        public async Task<IActionResult> GetPaged([FromQuery]int pageNo = 0, [FromQuery]int pageSize = 10, [FromQuery]ActivityType? type = null)
        {
            Result<IEnumerable<Activity>> res;
            if (type == null)
            {
                res = await this.activityService.GetPaged(pageNo, pageSize);
            }
            else
            {
                res = await this.activityService.GetPagedByType(type.Value, pageNo, pageSize);
            }
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpGet("{id:int}", Name = "GetSingle")]
        public async Task<IActionResult> GetSingle([FromRoute]int id)
        {
            var res = await this.activityService.GetById(id);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpGet("search", Name = "Search")]
        public async Task<IActionResult> Search([FromQuery]string term)
        {
            var res = await this.activityService.GetByTitle(term);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }
    }
}
