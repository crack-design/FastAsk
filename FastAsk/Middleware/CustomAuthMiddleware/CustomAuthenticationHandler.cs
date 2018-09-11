using Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FastAsk.Middleware.CustomAuthMiddleware
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private readonly IUserManager userManager;
        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserManager userManager)
        : base(options, logger, encoder, clock)
        {
            this.userManager = userManager;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (this.Context.Request.Headers["username"].ToString() == null || this.Context.Request.Headers["password"].ToString() == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail(new Exception("No username or password were provided")));
            }
            try
            {
                var user = await this.userManager.FindByUserNameAndPassword(this.Context.Request.Headers["username"].ToString(), this.Context.Request.Headers["password"].ToString());
                if (user != null)
                {
                    return await Task.FromResult(
                        AuthenticateResult.Success(
                            new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(Options.Identity), new AuthenticationProperties(), this.Scheme.Name)));
                }
                else
                {
                    return await Task.FromResult(AuthenticateResult.Fail(new Exception("Username or password is not valid")));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
