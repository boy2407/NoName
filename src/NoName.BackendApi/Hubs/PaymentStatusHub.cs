using Microsoft.AspNetCore.SignalR;

namespace NoName.BackendApi.Hubs
{
    public class PaymentStatusHub : Hub
    {
        public async Task JoinOrderGroup(string orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(orderId));
        }

        public async Task LeaveOrderGroup(string orderId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(orderId));
        }

        public static string GetGroupName(string orderId) => $"payment:order:{orderId}";
    }
}
