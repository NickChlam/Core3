using System.Threading.Tasks;
using DatingApp.Api.Models;

namespace DatingApp.Api.Data
{
    public interface IAuthRepository
    {
        // register user 
        Task<User> Register(User user, string Password);
        // login 
        Task<User> Login(string username, string Password);
        // does user exist 
        Task<bool> UserExisits(string username);

        

    }
}