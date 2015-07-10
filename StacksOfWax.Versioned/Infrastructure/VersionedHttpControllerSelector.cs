using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace StacksOfWax.Versioned.Infrastructure
{
    public class VersionedHttpControllerSelector : DefaultHttpControllerSelector
    {
        public VersionedHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        { }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor controllerDescriptor;
            var routeData = request.GetRouteData();

            object controller;
            object version;
            if (routeData.Values.TryGetValue("controller", out controller) && routeData.Values.TryGetValue("version", out version))
            {
                // this is the normal case using the VersionedApi instead of attribute routing
                var controllerName = string.Concat(controller, "V", version);
                var controllers = GetControllerMapping();
                controllers.TryGetValue(controllerName, out controllerDescriptor);
            }
            else
            {
                // attriubte routes already contain the version so use the base method
                controllerDescriptor = base.SelectController(request);
            }

            return controllerDescriptor;
        }

    }
}