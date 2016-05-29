using Microsoft.AspNet.Identity;
using ShareFun.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ShareFun.Classes
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
    {
        public SqlDatabase Database { get; set; }
        private UserTable userTable;

        public ApplicationUserStore() : this(new SqlDatabase())
        {
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
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<Object>(null);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        Task<ApplicationUser> IUserStore<ApplicationUser, string>.FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            List<ApplicationUser> result = userTable.GetUserByName(email);

            if (result != null && result.Count > 0)
            {
                return Task.FromResult<ApplicationUser>(result[0]);
            }

            return Task.FromResult<ApplicationUser>(null);
        }
    }
}