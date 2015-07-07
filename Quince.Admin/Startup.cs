using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Quince.Admin.Startup))]
namespace Quince.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
