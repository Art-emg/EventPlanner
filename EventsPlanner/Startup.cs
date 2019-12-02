using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventsPlanner.Startup))]
namespace EventsPlanner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
