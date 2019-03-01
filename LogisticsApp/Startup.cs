using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogisticsApp.Startup))]
namespace LogisticsApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
