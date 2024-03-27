using doan.Models;

namespace doan.Controllers.AdapterAuthenticator
{
    public interface IAuthenticationAdapter
    {
        Taikhoan Authenticate(string username, string password);
    }
}
