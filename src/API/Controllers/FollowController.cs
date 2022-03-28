using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> List(string username, string predicate)
        {
            var query = new List.Query { Predicate = predicate, UserName = username };
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> Toggle(string username)
        {
            var command = new FollowToggle.Command() { TargetUserName = username };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
