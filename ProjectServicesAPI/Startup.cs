using Microsoft.Owin;
using Owin;
using FixProUsApi.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.DependencyInjection;

[assembly: OwinStartup(typeof(FixProUsApi.Startup))]
namespace FixProUsApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //app.MapHubs("/chat", ChatHub);
        }
    }
}