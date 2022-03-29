using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {

        // GET api/<ProfileController>/5
        [HttpGet("{userName}")]
        public async Task<IActionResult> Details(string userName)
        {
            var request = new Details.Query { UserName = userName };
            var result = await Mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, [FromQuery] UserActivitiesParams UAParams)
        {
            var request = new ListActivities.Query { UserName = username, Params = UAParams };
            var result = await Mediator.Send(request);
            return HandlePagedResult(result);
        }
    }
}
