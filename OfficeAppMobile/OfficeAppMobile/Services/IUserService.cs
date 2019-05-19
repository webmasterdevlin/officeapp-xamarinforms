using System.Threading.Tasks;
using OfficeAppMobile.Models;

namespace OfficeAppMobile.Services
{
    public interface IUserService
    {
        Task<bool> SignupAsync(User user);
        Task<bool> LoginAsync(User user);

    }
}