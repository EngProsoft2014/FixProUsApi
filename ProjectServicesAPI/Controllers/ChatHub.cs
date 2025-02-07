//using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace FixProUsApi.Controllers
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

        public ChatHub()
        {
           
        }

        private void LogToFile(string message)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs.txt");
            File.AppendAllText(logPath, $"{DateTime.UtcNow}: {message}{Environment.NewLine}");
        }

        public async Task Send(string arg1, string arg2, string arg3, string arg4)
        {
            try
            {
                string toConnectionId = _users.FirstOrDefault(x => x.Value == arg4).Key;
                LogToFile($"name: {arg1}  message: {arg2} fromUserId: {arg3}  toUserId: {arg4} ConnectionId: {Context.ConnectionId} toConnectionId: {toConnectionId}");

                if (!string.IsNullOrEmpty(toConnectionId))
                {
                    await Clients.Client(toConnectionId).SendAsync("ReceiveMessage", arg1, arg2, arg3, arg4);
                }

                await Clients.Caller.SendAsync("ReceiveMessage", arg1, arg2, arg3, arg4);
                await Clients.All.SendAsync("ChangeUserData", arg1, arg2, arg3, arg4);

                LogToFile($"Sent toConnectionId: {toConnectionId} from ConnectionId: {Context.ConnectionId}");
            }
            catch (Exception ex)
            {
                LogToFile($"Error: {ex.Message} | InnerException: {ex.InnerException?.Message}");
            }
        }

        public async Task Connect(string userId)
        {
            LogToFile($"User connected: {userId}");

            _users.TryAdd(Context.ConnectionId, userId); // ✅ Thread-safe addition

            await Clients.All.SendAsync("ReceiveMessage", "Mohamed", "Mohamed Connected", "1", "2");
        }

        public async Task Disconnect(string fromUserId)
        {
            LogToFile($"User disconnecting: {fromUserId}");

            var whoLeft = _users.Where(x => x.Value == fromUserId).Select(x => x.Key).ToList();
            foreach (var connectionId in whoLeft)
            {
                _users.TryRemove(connectionId, out _);
            }

            await Clients.All.SendAsync("ReceiveMessage", "Mohamed", "Mohamed Disconnected", "1", "2");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_users.TryRemove(Context.ConnectionId, out string userId))
            {
                LogToFile($"User {userId} disconnected from {Context.ConnectionId}");
                await Clients.All.SendAsync("ReceiveMessage", "Mohamed", "User Disconnected", "1", "2");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }

    //public class ChatHub : Hub
    //{
    //    private static ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

    //    public void Send(string arg1, string arg2, string arg3, string arg4)
    //    {
    //        try
    //        {
    //            string toConnectionId = _users.FirstOrDefault(x => x.Value == arg4).Key;
    //            File.AppendAllText(HostingEnvironment.MapPath("~/logs.txt"), $"name: {arg1}  message: {arg2} fromUserId: {arg3}  toUserId: {arg4} Context.ConnectionId : {Context.ConnectionId} toConnectionId : {toConnectionId} {Environment.NewLine}");

    //            //////if (!string.IsNullOrEmpty(toConnectionId))
    //            //////    Clients.Client(toConnectionId).addMessage(name, message, fromUserId, toUserId);

    //            //////Clients.Client(Context.ConnectionId).addMessage(name, message, fromUserId, fromUserId);
    //            Clients.All.ChangeUserData(arg1, arg2, arg3, arg4);

    //            Clients.All.ReceiveMessage(arg1, arg2, arg3, arg4);


    //            File.AppendAllText(HostingEnvironment.MapPath("~/logs.txt"), $"toConnectionId: {toConnectionId} Context.ConnectionId: {Context.ConnectionId}{Environment.NewLine}");
    //        }
    //        catch (System.Exception ex)
    //        {
    //            File.AppendAllText(HostingEnvironment.MapPath("~/logs.txt"), $"error: {ex.Message} inner exception: {ex.InnerException}{Environment.NewLine}");
    //        }
    //    }
    //    public async Task Connect(string userId)
    //    {
    //        File.AppendAllText(HostingEnvironment.MapPath("~/logs.txt"), $"connect userId: {userId} {Environment.NewLine}");
    //        _users[Context.ConnectionId] = userId;
    //        await Clients.All.addMessage("Mohamed", "Mohamed Connected", "1", "2");
    //        File.AppendAllText(HostingEnvironment.MapPath("~/logs.txt"), $"connect userId: {userId} {Environment.NewLine}");
    //    }

    //    public async Task Disconnect(string fromUserId)
    //    {
    //        File.AppendAllText(HostingEnvironment.MapPath("~/logs.txt"), $"Disconnect fromUserId: {fromUserId} {Environment.NewLine}");
    //        string WhoConnectionLeft = _users.FirstOrDefault(x => x.Value == fromUserId).Key;
    //        _users.TryRemove(WhoConnectionLeft, out _);
    //        //_users.TryRemove(Context.ConnectionId, out _);
    //        await Clients.All.addMessage("Mohamed", "Mohamed closeleft", "1", "2");
    //        //await Clients.All.SendAsync("UserDisconnected", fromUserId);
    //    }
    //}
}