using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace StacksOfWax.Versioned.Infrastructure
{
    public class VersionedHttpControllerSelector : DefaultHttpControllerSelector
    {
        public VersionedHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        { }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();
            var routeData = request.GetRouteData();
            var controllerName = GetControllerName(routeData);
            var version = GetVersionFromRoute(routeData);

            //var version = GetVersionFromHeader(request);

            //var version = GetVersionFromQueryString(request);

            if (string.IsNullOrEmpty(version))
            {
                HttpControllerDescriptor controllerDescriptor;
                if (controllers.TryGetValue(controllerName, out controllerDescriptor))
                {
                    return controllerDescriptor;
                }
                return null;
            }

            var versionedControllerName = string.Concat(controllerName, "V", version);
            HttpControllerDescriptor versionedControllerDescriptor;
            if (controllers.TryGetValue(versionedControllerName, out versionedControllerDescriptor))
            {
                return versionedControllerDescriptor;
            }

            return null;
        }

        // TODO handle subroute, do same for version
        private string GetControllerName(IHttpRouteData routeData)
        {
            // Special handling for attribute routes
            const string attributeRouteKey = "MS_DirectRouteMatches";
            if (routeData.Values.ContainsKey(attributeRouteKey))
            {
                routeData = ((IEnumerable<IHttpRouteData>)routeData.Values["MS_DirectRouteMatches"]).First();
            }
            return routeData.Values["controller"].ToString();
        }

        private static string GetVersionFromRoute(IHttpRouteData routeData)
        {
            object version;
            if (routeData.Values.TryGetValue("version", out version))
            {
                return version.ToString();
            }
            return null;
        }

        private static string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var queryValues = request.GetQueryNameValuePairs();
            if (queryValues == null)
            {
                return null;
            }
            var versionPair = queryValues.SingleOrDefault(x => x.Key == "version");
            if (string.IsNullOrEmpty(versionPair.Value))
            {
                return null;
            }
            return versionPair.Value;
        }
        
        private static string GetVersionFromHeader(HttpRequestMessage request)
        {
            IEnumerable<string> versionHeaders;
            if (request.Headers.TryGetValues("x-version", out versionHeaders))
            {
                return versionHeaders.First();
            }
            return null;
        }

    }
}