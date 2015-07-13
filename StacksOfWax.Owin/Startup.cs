using System.Web.Http;
using Microsoft.Owin;
using Owin;
using StacksOfWax.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace StacksOfWax.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Run(context =>
            //{
            //    context.Response.ContentType = "text/plain";
            //    return context.Response.WriteAsync("Hello world!");
            //});
            
            //app.UseWelcomePage();

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
