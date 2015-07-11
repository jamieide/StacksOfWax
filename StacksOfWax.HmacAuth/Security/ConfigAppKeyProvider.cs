using System.Collections.Specialized;
using System.Web.Configuration;

namespace StacksOfWax.HmacAuth.Security
{
    public class ConfigAppKeyProvider : IAppKeyProvider
    {
        public string GetKey(string appId)
        {
            var apiKeys = WebConfigurationManager.GetSection("apiKeys") as NameValueCollection;
            if (apiKeys == null)
            {
                return null;
            }

            return apiKeys.Get(appId);
        }
    }
}