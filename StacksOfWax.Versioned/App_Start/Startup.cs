using System.Web.Http;
using Microsoft.Owin;
using Owin;
using StacksOfWax.Versioned;

[assembly: OwinStartup(typeof(Startup))]

namespace StacksOfWax.Versioned
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
