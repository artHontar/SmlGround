using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.Identity;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Interfaces;
using SmlGround.Models;

namespace SmlGround.Controllers
{
    [Authorize]
    public class SocialController : Controller
    {
        // GET: Social
        private IUserService UserService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }

        public ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult Profile()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel{Name = "few",Surname = "fef",Birthday = new DateTime(1999,04,30),City="Жлобин"};
            return View(profileViewModel);
        }
    }
}