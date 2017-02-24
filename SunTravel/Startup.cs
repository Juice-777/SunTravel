using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SunTravel.Startup))]
namespace SunTravel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
