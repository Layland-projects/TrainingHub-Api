using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrainingHub.API.Controllers
{
    [Authorize(Roles = "App.FullAccess")]
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController
    {
        //Add Stuff
    }
}
