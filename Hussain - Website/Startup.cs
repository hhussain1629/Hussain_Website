using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hussain___Website.Startup))]
namespace Hussain___Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
