using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        // GET: api/<ActivitiesController>
        [HttpGet]

        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            var request = new List.Query();
            return await Mediator.Send(request); 
        }

        // GET api/<ActivitiesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            var request = new Details.Query();
            request.Id = id;
            return await Mediator.Send(request);
        }

        // POST api/<ActivitiesController>
        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            var command = new Create.Command();
            command.Activity = activity;
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<ActivitiesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            var command = new Edit.Command();
            command.Activity = activity;
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<ActivitiesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var command = new Delete.Command();
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }

    }
}
