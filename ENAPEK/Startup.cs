using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ENAREK.Startup))]
namespace ENAREK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
