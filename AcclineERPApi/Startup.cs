
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using WebMatrix.WebData;

[assembly: OwinStartupAttribute(typeof(identity.Startup))]
namespace identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

