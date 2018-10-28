using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastAsk.Controllers
{
    [Route("api/testAuth")]
    [Authorize]
    public class TestAuthController : Controller
    {
        [AcceptVerbs("GET")]
        public IActionResult Index()
        {
            return StatusCode(200);
        }
    }
}