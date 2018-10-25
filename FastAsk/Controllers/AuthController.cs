using Authentication;
using Authentication.Implementations;
using FastAsk.Middleware.TokenGeneration;
using FastAsk.Models.AccountModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastAsk.Controllers
{
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IOptions<JwtAuthentication> jwtAuthentication;
        private readonly IUserManager userManager;

        public AuthController(IOptions<JwtAuthentication> jwtAuthentication,
            IUserManager userManager)
        {
            this.userManager = userManager;
            this.jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        [HttpPost, Route("login")]
        public async Task<string> Login([FromBody]LoginModel user)
        {
            var userData = await this.userManager.FindByUserNameAndPassword(user.UserName, PasswordHashHelper.HashPassword(user.Password));
            if (userData != null)
            {
                var token = new JwtSecurityToken(
                    issuer: jwtAuthentication.Value.ValidIssuer,
                    audience: jwtAuthentication.Value.ValidAudience,
                    claims: new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    },
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: jwtAuthentication.Value.SigningCredentials);


                var authorizationModel = new AuthorizationModel()
                {
                    authorizedUserModel = new AuthorizationModel.AuthorizedUserModel(userData.Id, userData.UserName, userData.Email)
                };
                authorizationModel.token = new JwtSecurityTokenHandler().WriteToken(token);
                return JsonConvert.SerializeObject(authorizationModel, new JsonSerializerSettings { Formatting = Formatting.Indented });
            }

            else
            {
                return JsonConvert.SerializeObject(new AuthorizationModel { errorMessage = "Wrong user name or password"}, new JsonSerializerSettings { Formatting = Formatting.Indented });
            }
        }
    }
}