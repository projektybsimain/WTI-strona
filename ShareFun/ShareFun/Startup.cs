using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShareFun.Startup))]
namespace ShareFun
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
