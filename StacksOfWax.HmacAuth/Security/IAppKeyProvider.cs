namespace StacksOfWax.HmacAuth.Security
{
    public interface IAppKeyProvider
    {
        string GetKey(string appId);
    }
}