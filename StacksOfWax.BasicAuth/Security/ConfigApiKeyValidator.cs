using System.Collections.Specialized;
using System.Web.Configuration;

namespace StacksOfWax.BasicAuth.Security
{
    public class ConfigApiKeyValidator : IApiKeyValidator
    {
        public bool IsValid(string username, string password)
        {
            var apiKeys = WebConfigurationManager.GetSection("apiKeys") as NameValueCollection;
            if (apiKeys == null)
            {
                return false;
            }

            var keyPassword = apiKeys.Get(username);
            if (keyPassword == null)
            {
                return false;
            }

            return keyPassword == password;
        }
    }
}