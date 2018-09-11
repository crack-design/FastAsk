using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FastAsk.Middleware.CustomAuthMiddleware
{
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity { get; set; } = new ClaimsIdentity();
    }
}
