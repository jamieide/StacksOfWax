using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using StacksOfWax.HmacAuth.Security;

[assembly: OwinStartup(typeof(StacksOfWax.HmacAuth.Startup))]

namespace StacksOfWax.HmacAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app
                .Use<HmacAuthOwinMiddleware>()
                .UseWebApi(config);
        }
    }
}
