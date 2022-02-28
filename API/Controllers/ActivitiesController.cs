using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        // GET: api/<ActivitiesController>
        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            var request = new List.Query();
            var result = await Mediator.Send(request);
            return HandleResult(result);
        }

        // GET api/<ActivitiesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var request = new Details.Query() { Id = id };
            var result = await Mediator.Send(request);
            return HandleResult(result);
        }

        // POST api/<ActivitiesController>
        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            var command = new Create.Command { Activity = activity };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        // PUT api/<ActivitiesController>/5
        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            var command = new Edit.Command() { Activity = activity };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        // DELETE api/<ActivitiesController>/5
        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var command = new Delete.Command() { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        // POST api/<ActivitiesController>/{id}/attend
        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            var command = new UpdateAttendance.Command() { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }



    }
}
