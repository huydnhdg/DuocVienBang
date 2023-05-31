using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTV.Startup))]
namespace MTV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
