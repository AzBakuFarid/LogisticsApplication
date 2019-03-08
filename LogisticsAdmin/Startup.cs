using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogisticsAdmin.Startup))]
namespace LogisticsAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
