using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bookmark.Startup))]
namespace Bookmark
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
