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
    public class RoleStore : IRoleStore<AppRole>
    {
        private readonly string connectionString;

        public RoleStore(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                role.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [AppRole] ([Name])
                    VALUES (@{nameof(AppRole.Name)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [AppRole] SET
                    [Name] = @{nameof(AppRole.Name)}
                    WHERE [Id] = @{nameof(AppRole.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [AppRole] WHERE [Id] = @{nameof(AppRole.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(AppRole role, string Name, CancellationToken cancellationToken)
        {
            role.Name = Name;
            return Task.FromResult(0);
        }

        public async Task<AppRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<AppRole>($@"SELECT * FROM [AppRole]
                    WHERE [Id] = @{nameof(roleId)}", new { roleId });
            }
        }

        public async Task<AppRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<AppRole>($@"SELECT * FROM [AppRole]
                    WHERE [Name] = @{nameof(normalizedRoleName)}", new { normalizedRoleName });
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
