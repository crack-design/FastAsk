using Authentication;
using Authentication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAsk.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IUserManager userManager;

        public UserManagerController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            var isUserLoginOccupied = this.userManager.FindByNameAsync(model.UserName) == null;
            if (!this.ModelState.IsValid)
            {
                throw new Exception("Wrong data");
            }
            else if (isUserLoginOccupied)
            {
                return new JsonResult("User name is occupied");
            }
            else
            {
                var result = await this.userManager.CreateUserAsync(model);
                if (result > 0)
                {
                    return new JsonResult("User created");
                }
                else
                {
                    return new StatusCodeResult(500);
                }
            }
        }
    }
}
