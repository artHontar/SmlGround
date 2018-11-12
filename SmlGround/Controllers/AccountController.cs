﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.Identity;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;
using SmlGround.DLL.Interfaces;
using SmlGround.Models;
using SmlGround.DLL.Service;
using SmlGround.SMTP;

namespace SmlGround.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }
        
        public ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
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
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
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
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Birthday = model.Birthday,
                    Password = model.Password,
                    Name = model.Name,
                    Surname = model.Surname,
                    Role = "user"
                };
                string id = await UserService.Create(userDto);
                if (id != null && id != "Пользователь с таким логином уже существует")
                {
                    string subject = "Подтверждение email";
                    string messege = string.Format("Для завершения регистрации перейдите по ссылке:" +
                                     "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>",
                        Url.Action("ConfirmEmail", "Account", new {Token = id, Email = userDto.Email},
                        Request.Url.Scheme));
                    Sender sender = new Sender("Web Registration", userDto.Email);
                    sender.SendMessage(subject, messege);
                    return RedirectToAction("Confirm", "Account", new { Email = userDto.Email });
                    
                }
                else if(id != null)
                    ModelState.AddModelError("Email", "Пользователь с таким логином уже существует");
                else
                {
                    ModelState.AddModelError("Error", "Что");
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public string Confirm(string Email)
        {
            return "На почтовый адрес " + Email + " Вам высланы дальнейшие" +
                   "инструкции по завершению регистрации";
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            OperationDetails operationDetails = await UserService.ConfirmEmail(Token, Email);
            
            if (operationDetails.Succeed)
            {
                return RedirectToAction("Login", "Account", new { Email = Email });
            }

            else if(operationDetails.Message == "Повторите")
                return RedirectToAction("Confirm", "Account", new { Email = Email });
            else
                return RedirectToAction("Confirm", "Account", new { Email = "" });
        
        }

        //await SetInitialDataAsync();

        //private async Task SetInitialDataAsync()
        //{
        //    await UserService.SetInitialData(new UserDTO
        //    {
        //        Email = "artemgontar16@gmail.com",
        //        UserName = "artemgontar16",
        //        Password = "123456",
        //        Birthday = new DateTime(1999, 04, 30),
        //        Name = "Artem",
        //        Surname = "Gontar",
        //        Role = "admin",
        //    }, new List<string> { "user", "admin" });
        //}
    }
}