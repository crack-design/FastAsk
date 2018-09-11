using Authentication.ViewModels;
using System;
using System.Threading.Tasks;

namespace Authentication
{
    public interface IUserManager
    {
        Task<int> CreateUserAsync(UserViewModel model);
        Task<UserViewModel> FindByNameAsync(string userName);
        Task<UserViewModel> FindByIdAsync(int id);
        Task<UserViewModel> FindByUserNameAndPassword(string username, string hashedPassword);
    }
}
