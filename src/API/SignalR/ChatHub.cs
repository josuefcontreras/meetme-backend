using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public interface IChatClient
    {
        Task ReceiveComment(CommentDTO comment);
        Task LoadComments(List<CommentDTO> comments);
    }
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task SendComment(Create.Command command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                await Clients.Group(command.ActivityId.ToString())
                    .ReceiveComment(result.Value);
            }
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var result = await _mediator.Send(new List.Query() { ActivityId = Guid.Parse(activityId) });

            await Clients.Caller.LoadComments(result.Value);
        }
    }
}
