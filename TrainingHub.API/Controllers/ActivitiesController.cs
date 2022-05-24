using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using TrainingHub.BusinessLogic.DTOs.Activity;
using TrainingHub.Infrastructure.Abstractions;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.API.Controllers
{
    [Authorize(Roles = "App.FullAccess")]
    [ApiController]
    [Route("api/[controller]")]
    [RequiredScope("AngularApp")]
    public class ActivitiesController : ControllerBase
    {
        private const string controllerName = "Activities";
        private readonly IActivityService activityService;
        public ActivitiesController(IActivityService activityService)
        {
            this.activityService = activityService;
        }

        [HttpGet("", Name = "GetActivitiesPaged")]
        public async Task<IActionResult> GetActivitiesPagedAsync([FromQuery]int pageNo = 0, [FromQuery]int pageSize = 10, [FromQuery]ActivityType? type = null)
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

        [HttpGet("{id:int}", Name = "GetActivitiesSingle")]
        public async Task<IActionResult> GetActivitiesSingleAsync([FromRoute]int id)
        {
            var res = await this.activityService.GetById(id);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPut("{id:int}", Name = "UpdateActivity")]
        public async Task<IActionResult> UpdateActivityAsync([FromRoute]int id, [FromBody] UpdateActivity_DTO dto)
        {
            var res = await this.activityService.Update(id, 
                dto.Title,
                dto.Description, 
                (ActivityType)dto.ActivityType, 
                dto.IsBodyWeight,
                dto.Image);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpGet("search", Name = "SearchActivities")]
        public async Task<IActionResult> SearchActivitiesAsync([FromQuery]string term, [FromQuery]int pageNo = 0, [FromQuery]int pageSize = 10)
        {
            var res = await this.activityService.SearchPaged(term, pageNo, pageSize);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpDelete("{id:int}/disable", Name = "DisableActivity")]
        public async Task<IActionResult> DisableActivityAsync([FromRoute]int id)
        {
            var res = await this.activityService.Disable(id);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("{id:int}/enable", Name = "EnableActivity")]
        public async Task<IActionResult> EnableActivityAsync([FromRoute]int id)
        {
            var res = await this.activityService.Enable(id);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }
    }
}
