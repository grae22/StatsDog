using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StatsDog.Startup))]
namespace StatsDog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
