using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainingHub.BusinessLogic.DTOs;
using TrainingHub.Infrastructure.Abstractions;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("", Name = "GetPaged")]
        public async Task<IActionResult> GetPagedAsync(CancellationToken cancellationToken, [FromQuery] int pageSize = 10, [FromQuery] int pageNum = 0)
        {
            var res = await this.userService.GetUsersAsync(pageSize, pageNum, cancellationToken);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet("{id:int}", Name = "Get")]
        public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var res = await this.userService.GetUserAsync(id, cancellationToken);
            return res.Status == ResultStatus.Success ? Ok(res) : NotFound();
        }

        [HttpPost("signup", Name = "SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUp_DTO dto)
        {
            var res = await this.userService.CreateUserAsync(
                dto.Title,
                dto.FirstName,
                dto.LastName,
                "",
                dto.Email,
                dto.Password,
                (UserRole)dto.Role);

            if (res.Status == ResultStatus.Failed)
            {
                return BadRequest(res);
            }

            return CreatedAtRoute("Get", new { id = res.Data.Id }, res.Data);
        }

        [HttpPost("signin", Name = "SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] SignIn_DTO dto)
        {
            // update to add a sign in via email and password.
            var res = await this.userService.SignIn(dto.Email, dto.Password);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("signout", Name = "SignOut")]
        public async Task<IActionResult> SignOutAsync([FromBody] SignOut_DTO dto)
        {
            var res = await this.userService.SignOut(dto.Id);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("forgotPassword", Name = "ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPassword_DTO dto)
        {
            var res = await this.userService.ForgotPassword(dto.Email);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("updateEmail", Name = "UpdateEmail")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UpdateEmail_DTO dto)
        {
            var res = await this.userService.UpdateEmailAsync(dto.Id, dto.Email);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("updatePassword", Name = "UpdatePassword")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePassword_DTO dto)
        {
            var res = await this.userService.UpdatePasswordAsync(dto.Id, dto.Password);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute]int id, [FromBody] UpdateUser_DTO dto)
        {
            var res = await this.userService.UpdateUserAsync(id, dto.Title, dto.FirstName, dto.LastName);
            return res.Status == ResultStatus.Success ? Ok(res) : BadRequest(res);
        }
    }
}
