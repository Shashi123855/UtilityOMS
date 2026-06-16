using Microsoft.AspNetCore.SignalR;

namespace UtilityOMS.API.Hubs
{
    public class OutageHub :Hub
    {
        // Client calls this to join the map group
        public async Task JoinMapGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "MapViewers");
        }

        // Client calls this to leave the map group
        public async Task LeaveMapGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "MapViewers");
        }
    }
}
