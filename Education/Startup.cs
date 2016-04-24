using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Education.Startup))]
namespace Education
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
