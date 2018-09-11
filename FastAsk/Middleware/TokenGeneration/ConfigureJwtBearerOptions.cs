using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FastAsk.Middleware.TokenGeneration
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private IOptions<JwtAuthentication> jwtAuthentication;

        public ConfigureJwtBearerOptions(IOptions<JwtAuthentication> jwtAuthentication)
        {
            this.jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var jwtAuthenticationValues = jwtAuthentication.Value;

            options.ClaimsIssuer = jwtAuthenticationValues.ValidIssuer;
            options.IncludeErrorDetails = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtAuthenticationValues.ValidIssuer,
                ValidAudience = jwtAuthenticationValues.ValidAudience,
                IssuerSigningKey = jwtAuthenticationValues.SymmetricSecurityKey,
                NameClaimType = ClaimTypes.NameIdentifier
            };
        }
    }
}
