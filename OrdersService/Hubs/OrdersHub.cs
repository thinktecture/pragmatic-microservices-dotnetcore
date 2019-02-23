
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Linq;

namespace OrdersService.Hubs
{
    [Authorize]
    public class OrdersHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            var subjectId = identity?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            Groups.AddToGroupAsync(Context.ConnectionId, subjectId);

            return base.OnConnectedAsync();
        }
    }
}
