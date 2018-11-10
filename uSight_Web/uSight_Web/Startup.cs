using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(uSight_Web.Startup))]
namespace uSight_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
