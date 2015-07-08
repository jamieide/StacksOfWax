using System.Web.Http;
using System.Web.Http.Dispatcher;
using StacksOfWax.Versioned.Infrastructure;

namespace StacksOfWax.Versioned
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "VersionedApi",
                routeTemplate: "api/v{version}/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.Services.Replace(typeof(IHttpControllerSelector), new VersionedHttpControllerSelector(config));
        }
    }
}
