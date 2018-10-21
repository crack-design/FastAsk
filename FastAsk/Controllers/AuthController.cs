using Authentication;
using Authentication.Implementations;
using FastAsk.Middleware.TokenGeneration;
using FastAsk.Models.AccountModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (this.userManager.FindByUserNameAndPassword(user.UserName, PasswordHashHelper.HashPassword(user.Password)) != null)
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