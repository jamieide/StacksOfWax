namespace StacksOfWax.BasicAuth.Security
{
    public interface IApiKeyValidator
    {
        bool IsValid(string username, string password);
    }
}