using Application.Photos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PhotosController: BaseApiController        
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new Delete.Command() { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            var command = new SetMain.Command() { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
