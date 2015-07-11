using System;
using System.IO;
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
        private readonly IAppKeyProvider _appKeyProvider;

        public HmacAuthOwinMiddleware(OwinMiddleware next, IAppKeyProvider appKeyProvider) : base(next)
        {
            _appKeyProvider = appKeyProvider;
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
                string requestAppId;
                if (TryIsValidRequest(request, out requestAppId))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, requestAppId)
                    };
                    var identity = new ClaimsIdentity(claims, "HMAC");
                    request.User = new ClaimsPrincipal(identity);
                }
            }

            await Next.Invoke(context);
        }

        private bool TryIsValidRequest(IOwinRequest request, out string requestAppId)
        {
            requestAppId = null;

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
            requestAppId = parameters[0];
            var requestSignature = parameters[1];
            var requestTimeStamp = parameters[2];

            var apiKey = _appKeyProvider.GetKey(requestAppId);
            if (apiKey == null)
            {
                return false;
            }

            // TODO check replay request

            // Compute hash
            var requestContent = string.Empty;
            var hash = ComputeHash(request);
            if (hash != null)
            {
                requestContent = Convert.ToBase64String(hash);
            }

            var requestMethod = request.Method;
            var requestUri = WebUtility.UrlEncode(request.Uri.ToString().ToLowerInvariant());
            var hmacData = string.Concat(requestAppId, requestMethod, requestUri, requestTimeStamp, requestContent);

            var secretKeyBytes = Convert.FromBase64String(apiKey);
            var signature = Encoding.UTF8.GetBytes(hmacData);

            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var signatureString = Convert.ToBase64String(signatureBytes);
                return requestSignature.Equals(signatureString, StringComparison.Ordinal);
            }
        }

        private byte[] ComputeHash(IOwinRequest request)
        {
            using (var ms = new MemoryStream())
            using (var md5 = MD5.Create())
            {
                request.Body.CopyTo(ms);
                var body = ms.ToArray();
                if (body.Length > 0)
                {
                    return md5.ComputeHash(body);
                }
            }
            return null;
        }
    }
}