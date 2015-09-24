using Microsoft.Owin;
using Owin;
using SimpleTracking.Web.App_Start;

[assembly: OwinStartup(typeof(SimpleTracking.Web.Startup))]
namespace SimpleTracking.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
