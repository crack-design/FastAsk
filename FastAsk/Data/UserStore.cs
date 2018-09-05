using Dapper;
using FastAsk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FastAsk.Data
{
    public class UserStore : IUserStore<AppUser>
    {
        private readonly string connectionString;
        public UserStore(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [AppUser] ([UserName], [Email], [EmailConfirmed], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed])
                    VALUES (@{nameof(AppUser.UserName)}, @{nameof(AppUser.Email)}, @{nameof(AppUser.EmailConfirmed)}, @{nameof(AppUser.PasswordHash)},
                    @{nameof(AppUser.PhoneNumber)}, @{nameof(AppUser.PhoneNumberConfirmed)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [AppUser] WHERE [Id] = @{nameof(AppUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<AppUser>($@"SELECT * FROM [AppUser]
                    WHERE [Id] = @{nameof(userId)}", new { userId });
            }
        }

        public async Task<AppUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<AppUser>($@"SELECT * FROM [AppUser]
                    WHERE [UserName] = @{nameof(userName)}", new { userName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
