using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RenginiuSistema.Startup))]
namespace RenginiuSistema
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
