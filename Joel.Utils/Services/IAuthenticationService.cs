using System.Threading.Tasks;

namespace Joel.Utils.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Register(string username, string password);
    }
}
