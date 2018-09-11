using Authentication;
using Authentication.Implementations;
using FastAsk.Middleware.TokenGeneration;
using FastAsk.Models.AccountModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FastAsk.Controllers
{
    public class UserController : Controller
    {
        private readonly IOptions<JwtAuthentication> jwtAuthentication;
        private readonly IUserManager userManager;

        public UserController(IOptions<JwtAuthentication> jwtAuthentication, IUserManager userManager)
        {
            this.userManager = userManager;
            this.jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public IActionResult GenerateToken([FromForm] GenerateTokenModel model)
        {
            if (this.userManager.FindByUserNameAndPassword(model.UserName, PasswordHashHelper.HashPassword(model.Password)) != null)
            {
                var token = new JwtSecurityToken(
                    issuer: jwtAuthentication.Value.ValidIssuer,
                    audience: jwtAuthentication.Value.ValidAudience,
                    claims: new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    },
                    expires: DateTime.UtcNow.AddDays(7),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: jwtAuthentication.Value.SigningCredentials);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            else
            {
                return BadRequest("Username or password is invalid");
            }
        }
    }
}
