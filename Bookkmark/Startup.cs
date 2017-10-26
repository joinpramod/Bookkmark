using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bookkmark.Startup))]
namespace Bookkmark
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
