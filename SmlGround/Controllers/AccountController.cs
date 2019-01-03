using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Owin.Security;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;
using SmlGround.DLL.Interfaces;
using SmlGround.Filters;
using SmlGround.Models;
using SmlGround.SMTP;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmlGround.Controllers
{
    [LogInfo]
    [NullException]
    public class AccountController : Controller
    {
        private IUserService UserService; //=> HttpContext.GetOwinContext().GetUserManager<IUserService>();
        //public ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public AccountController(IUserService userService)
        {
            UserService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        { 
            if (ModelState.IsValid)
            {
                var userDto = new UserConfirmDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.AuthenticateAsync(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Profile", "Social");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistrationModel model)
        {
            //await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserRegistrationDTO userDto = Mapper.Map<RegistrationModel, UserRegistrationDTO>(model);
                //userDto.Role = "user";
                string id = await UserService.CreateAsync(userDto);
                if (id != null && id != "Пользователь с таким логином уже существует")
                {
                    var confirmEmail = new ConfirmEmail(Url.Action("ConfirmEmail", "Account",
                        new { Token = id, Email = userDto.Email },Request.Url.Scheme));
                    
                    Sender sender = new Sender("Web Registration", userDto.Email);
                    sender.SendMessage(confirmEmail);
                    return RedirectToAction("Confirm", "Account", new { Email = userDto.Email });
                    
                }
                if(id != null)
                    ModelState.AddModelError("Email", "Пользователь с таким логином уже существует");
                else
                    ModelState.AddModelError("Error", "Что");
            }
            return View(model);
        }

        [AllowAnonymous]
        [NonDirectAccess]
        public string Confirm(string Email)
        {
            return "На почтовый адрес " + Email + " Вам высланы дальнейшие" +
                   "инструкции по завершению регистрации";
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            OperationDetails operationDetails = await UserService.ConfirmEmailAsync(Token, Email);
            
            if (operationDetails.Succeed)
            {
                UserConfirmDTO userDto = new UserConfirmDTO() {Email = Email};
                ClaimsIdentity claim = await UserService.AutoAuthenticateAsync(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                    return RedirectToAction("Login", "Account", new { Email = Email });
                }
                
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                return RedirectToAction("Profile", "Social");
            
            }
            if(operationDetails.Message == "Повторите")
                return RedirectToAction("Confirm", "Account", new { Email = Email });
            return RedirectToAction("Confirm", "Account", new { Email = "" });
        
        }
        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialDataAsync(new UserRegistrationDTO
            {
                Email = "artemgontar16@gmail.com",
                UserName = "artemgontar16",
                Password = "123456",
                Birthday = new DateTime(1999, 04, 30),
                Name = "Artem",
                Surname = "Gontar",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }
    }
}