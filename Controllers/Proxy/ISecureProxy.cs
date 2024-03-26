namespace doan.Controllers.Proxy
{
    public interface ISecureProxy
    {
            bool Authenticate(string username, string password);
        bool IsAuthorized(string username, string role);

    }
}
