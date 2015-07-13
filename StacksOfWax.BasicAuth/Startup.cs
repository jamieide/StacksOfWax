using System.Web.Http;
using Microsoft.Owin;
using Owin;
using StacksOfWax.BasicAuth;
using StacksOfWax.BasicAuth.Security;

[assembly: OwinStartup(typeof(Startup))]

namespace StacksOfWax.BasicAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app
                .Use<BasicAuthOwinMiddleware>(new DemoApiKeyValidator())
                .UseWebApi(config);
        }
    }
}
