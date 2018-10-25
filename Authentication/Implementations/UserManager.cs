using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Authentication.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Authentication.Implementations
{
    public class UserManager : IUserManager
    {
        public IConfiguration Configuration { get; }
        private readonly string connectionString;
        public UserManager(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.connectionString = Configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<UserViewModel> FindByIdAsync(int id)
        {
            var userModel = new UserViewModel();
            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<UserViewModel>($@"SELECT * FROM [AppUser]
                            WHERE [Id] = @{nameof(id)}", new { id });
            }
        }

        public async Task<UserViewModel> FindByNameAsync(string userName)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<UserViewModel>($@"SELECT * FROM [AppUser]
                            WHERE [UserName] = @{nameof(userName)}", new { userName });
            }
        }

        public async Task<UserViewModel> FindByUserNameAndPassword(string username, string hashedPassword)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<UserViewModel>($@"SELECT * FROM [AppUser]
                            WHERE [UserName] = @{nameof(username)} AND [PasswordHash] = @{nameof(hashedPassword)}", new { username, hashedPassword });
            }
        }

        public async Task<int> CreateUserAsync(UserViewModel model)
        {
            model.PasswordHash = PasswordHashHelper.HashPassword(model.PasswordHash);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                model.Id = await connection.QueryFirstOrDefaultAsync<int>($@"INSERT INTO [AppUser] ([UserName],[Email],[PasswordHash])
                            VALUES (@{nameof(UserViewModel.UserName)}, @{nameof(UserViewModel.Email)}, @{nameof(UserViewModel.PasswordHash)});
                            SELECT CAST(SCOPE_IDENTITY() as int)", model);
            }
            return model.Id;
        }
    }
}
