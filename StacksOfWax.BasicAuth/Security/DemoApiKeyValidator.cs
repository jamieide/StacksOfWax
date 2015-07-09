namespace StacksOfWax.BasicAuth.Security
{
    public class DemoApiKeyValidator : IApiKeyValidator
    {
        public bool IsValid(string username, string password)
        {
            return username == "tester" && password == "secret";
        }
    }
}