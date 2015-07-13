using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace StacksOfWax.BasicAuth.Security
{
    public class BasicAuthOwinMiddleware : OwinMiddleware
    {
        private readonly IApiKeyValidator _apiKeyValidator;

        public BasicAuthOwinMiddleware(OwinMiddleware next, IApiKeyValidator apiKeyValidator)
            : base(next)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.OnSendingHeaders(state =>
            {
                var owinResponse = (OwinResponse)state;

                if (owinResponse.StatusCode == (int)HttpStatusCode.Unauthorized && !owinResponse.Headers.ContainsKey("WWW-Authenticate"))
                {
                    owinResponse.Headers.Add("WWW-Authenticate", new[] { "Basic" });
                }
            }, response);

            AuthenticationHeaderValue authHeader;
            if (AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out authHeader))
            {
                if (authHeader.Scheme == "Basic")
                {
                    var parameters = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');

                    var username = parameters[0];
                    var password = parameters[1];

                    if (_apiKeyValidator.IsValid(username, password))
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, username)
                        };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        request.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await Next.Invoke(context);
        }
    }
}