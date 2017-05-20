using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MarketOnline.Startup))]
namespace MarketOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
