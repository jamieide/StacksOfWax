using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace StacksOfWax.HmacAuth.Security
{
    public class HmacAuthOwinMiddleware : OwinMiddleware
    {
        public HmacAuthOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.OnSendingHeaders(state =>
            {
                var owinResponse = (OwinResponse) state;

                if (owinResponse.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    // TODO Not sure why this is already present if I run multiple unit tests using the same in-memory server instance
                    if (!owinResponse.Headers.ContainsKey("Authentication"))
                    {
                        // custom SOW scheme
                        owinResponse.Headers.Add("Authentication", new[] {"SOW"});
                    }
                }
            }, response);

            AuthenticationHeaderValue authHeader;
            if (AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out authHeader) && authHeader.Scheme == "SOW")
            {
                if (IsValidRequest(request))
                {
                    var claims = new[]
                    {
                        // TODO
                        new Claim(ClaimTypes.Name, "APPID GOES HERE")
                    };
                    var identity = new ClaimsIdentity(claims, "HMAC");
                    request.User = new ClaimsPrincipal(identity);
                }
            }

            await Next.Invoke(context);
        }

        private bool IsValidRequest(IOwinRequest request)
        {
            // TODO move to config
            const string appId = "2fad4c19-0fcc-429e-8f41-e21b73db75cd";
            const string apiKey = "SjSa9vT4QWNXERDcAde4rjtc4tq4ZNojjaq7JoZ+81w=";

            AuthenticationHeaderValue authHeader;
            if (!AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out authHeader) || authHeader.Scheme != "SOW")
            {
                return false;
            }

            var parameters = authHeader.Parameter.Split(':');
            if (parameters.Length != 3)
            {
                return false;
            }
            var requestAppId = parameters[0];
            var requerstSignature = parameters[1];
            var requestTimeStamp = parameters[2];

            if (appId != requestAppId)
            {
                return false;
            }

            // TODO check replay request

            // Compute hash
            var hash = ComputeHash(request.Body);

            return true;

        }

        private byte[] ComputeHash()
        {
            
        }
    }
}