using FrontToBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FrontToBack.SignalRChat
{
    public class HubChat:Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public HubChat(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("dd.MM.yyyy"));
        }


        public override  Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectedId = Context.ConnectionId;
                _userManager.UpdateAsync(user);
                Clients.All.SendAsync("UserConnect", user.Id);

            }

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectedId = null;
                _userManager.UpdateAsync(user);
                Clients.All.SendAsync("DisConnect", user.Id);

            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
