using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainingHub.BusinessLogic.DTOs;
using TrainingHub.Infrastructure.Abstractions;
using static TrainingHub.Models.Enums.Enums;
using TrainingHub.API.CustomAttributes;

namespace TrainingHub.API.Controllers
{
    [Authorize(Roles = "App.FullAccess")]
    [ApiController]
    [Route("api/[controller]")]
    [RequiredScope("AngularApp")]
    public class UsersController : ControllerBase
    {
        const string controllerName = "Users";
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("", Name = "GetUsersPaged")]
        public async Task<IActionResult> GetUsersPagedAsync(CancellationToken cancellationToken, [FromQuery] int pageSize = 10, [FromQuery] int pageNum = 0)
        {
            var res = await this.userService.GetUsersAsync(pageSize, pageNum, cancellationToken);
            return res != null ? Ok(res) : BadRequest(res);
        }

        [HttpGet("{id:int}", Name = "Get")]
        public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var res = await this.userService.GetUserAsync(id, cancellationToken);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpGet("azure/{azureId:guid}", Name = "GetByAzureId")]
        public async Task<IActionResult> GetByAzureId([FromRoute]Guid azureId, CancellationToken cancellationToken)
        {
            var res = await this.userService.GetUserByAzureIdAsync(azureId, cancellationToken);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("register", Name = "Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Register_DTO dto, CancellationToken cancellationToken)
        {
            var res = await this.userService.Register(dto.Id, dto.ContactNumber, dto.Email, dto.FirstName, dto.LastName, dto.Role, cancellationToken);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }
    }
}
