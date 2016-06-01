using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using ShareFun.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ShareFun.Classes
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserLockoutStore<ApplicationUser, string>, IUserTwoFactorStore<ApplicationUser, string>
    {
        public SqlDatabase Database { get; set; }
        private UserTable userTable;

        public ApplicationUserStore() : this(new SqlDatabase())
        {
        }

        public static string GetUserIDByName(ApplicationUserManager manager, IPrincipal user)
        {
            Task<ApplicationUser> applicationUser = manager.FindByNameAsync(user.Identity.Name);
            return applicationUser.Result.Id;
        }

        public ApplicationUserStore(SqlDatabase database)
        {
            Database = database;
            userTable = new UserTable(Database);
        }

        public Task CreateAsync(ApplicationUser user)
        {
            userTable.Insert(user);
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.CloseConnection();
                Database = null;
            }
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            userTable.UpdatePassword(user);
            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            string passwordHash = userTable.GetPasswordHash(user.Id);
            return Task.FromResult<string>(passwordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            bool hasPassword = !string.IsNullOrEmpty(userTable.GetPasswordHash(user.Id));
            return Task.FromResult<bool>(Boolean.Parse(hasPassword.ToString()));
        }

        Task<ApplicationUser> IUserStore<ApplicationUser, string>.FindByIdAsync(string userId)
        {
            List<ApplicationUser> result = userTable.GetUserByID(userId);
            if (result != null && result.Count > 0)
            {
                return Task.FromResult<ApplicationUser>(result[0]);
            }
            return Task.FromResult<ApplicationUser>(null);
        }

        Task<ApplicationUser> IUserStore<ApplicationUser, string>.FindByNameAsync(string userName)
        {
            List<ApplicationUser> result = userTable.GetUserByName(userName);
            if (result != null && result.Count > 0)
            {
                return Task.FromResult<ApplicationUser>(result[0]);
            }
            return Task.FromResult<ApplicationUser>(null);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            userTable.SetEmailConfirmed(user, confirmed);
            return Task.FromResult<object>(null);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            List<ApplicationUser> result = userTable.GetUserByEmail(email);
            if (result != null && result.Count > 0)
            {
                return Task.FromResult<ApplicationUser>(result[0]);
            }
            return Task.FromResult<ApplicationUser>(null);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task<bool>.FromResult(false);
        }
    }
}