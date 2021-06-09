using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(ChildProfiles.Startup))]
namespace ChildProfiles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
             ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
