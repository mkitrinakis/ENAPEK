using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ENAPEK.Startup))]
namespace ENAPEK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
