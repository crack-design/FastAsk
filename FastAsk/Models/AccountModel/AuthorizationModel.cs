using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAsk.Models.AccountModel
{
    internal class AuthorizationModel
    {
        public string token { get; internal set; }
        public string errorMessage { get; internal set; }
        public AuthorizedUserModel authorizedUserModel { get; internal set; }

        internal class AuthorizedUserModel
        {
            public AuthorizedUserModel(int id, string login, string email)
            {
                this.Id = id;
                this.Email = email;
                this.Login = login;
            }
            public int Id { get; }
            public string Login { get; }
            public string Email { get; }
        }
    }
}
