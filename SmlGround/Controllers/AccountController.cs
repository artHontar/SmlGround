using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.Identity;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;
using SmlGround.DLL.Interfaces;
using SmlGround.Models;
using SmlGround.SMTP;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmlGround.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();
        public ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

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
                    return RedirectToAction("Profile", "Social");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
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
                UserDTO userDto = new UserDTO() {Email = Email};
                ClaimsIdentity claim = await UserService.AutoAuthenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                    return RedirectToAction("Login", "Account", new { Email = Email });
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
            else if(operationDetails.Message == "Повторите")
                return RedirectToAction("Confirm", "Account", new { Email = Email });
            else
                return RedirectToAction("Confirm", "Account", new { Email = "" });
        
        }
    }
}