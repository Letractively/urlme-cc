using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(urlme.site.Startup))]
namespace urlme.site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
