using API.Models;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse> RegisterUser(RegistrationModel model);
        Task<ApiResponse> Login(LoginModel model);
        Task<ApiResponse> GetUser(int UserId);
        Task<ApiResponse> GetUsers();
    }
}
