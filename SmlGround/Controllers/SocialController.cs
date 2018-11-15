using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using SmlGround.DataAccess.Identity;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;
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

        public ActionResult Profile(string id)
        {
            //UserDTO userDto = await UserService.FindById();
            if (id == null)
            {
                id = HttpContext.User.Identity.GetUserId();
            }

            ProfileDTO profileDto = UserService.FindProfile(id);
                ProfileViewModel profileViewModel = new ProfileViewModel
                {
                    Id = profileDto.Id,
                    Avatar = profileDto.Avatar,
                    Name = profileDto.Name,
                    Surname = profileDto.Surname,
                    Birthday = profileDto.Birthday,
                    City = profileDto.City,
                    PlaceOfStudy = profileDto.PlaceOfStudy,
                    Skype = profileDto.Skype
                };
            if (profileViewModel.Avatar != null)
            {
                ViewBag.Image = Convert.ToBase64String(profileViewModel.Avatar);
            }
            if (profileViewModel.Id == HttpContext.User.Identity.GetUserId())
                {
                    profileViewModel.IsCurrentUserProfile = true;
                }
                return View(profileViewModel);
        }
        public ActionResult Edit(string id)
        {
            ProfileDTO profileDto = UserService.FindProfile(id);
            ProfileViewModel profileViewModel = new ProfileViewModel
            {
                Id = profileDto.Id,
                Avatar = profileDto.Avatar,
                Name = profileDto.Name,
                Surname = profileDto.Surname,
                Birthday = profileDto.Birthday,
                City = profileDto.City,
                PlaceOfStudy = profileDto.PlaceOfStudy,
                Skype = profileDto.Skype
            };

            return View("Edit",profileViewModel);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Birthday,City,PlaceOfStudy")]ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                ProfileDTO profileDto = new ProfileDTO()
                {
                    Id = profileViewModel.Id,
                    Name = profileViewModel.Name,
                    Surname = profileViewModel.Surname,
                    Birthday = profileViewModel.Birthday,
                    City = profileViewModel.City,
                    PlaceOfStudy = profileViewModel.PlaceOfStudy,
                    Skype = profileViewModel.Skype
                };
                
                UserService.Update(profileDto);
            }
            //Update Profile
            //UserDTO userDto = await UserService.FindById();
            return RedirectToAction("Profile", "Social");
        }
        [HttpPost]
        public ActionResult EditAvatar(string id, HttpPostedFileBase uploadImage)
        {
            ProfileDTO profileDto = UserService.FindProfile(id);

            if (ModelState.IsValid) //&& uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                profileDto.Avatar = imageData;
                UserService.UpdateAvatar(profileDto);
                return Json(imageData);
                //return Content(base64Image);
            }
            //ProfileDTO profileDto = new ProfileDTO()
            //{
            //    Id = profileViewModel.Id,
            //    Avatar = profileViewModel.Avatar,
            //    Name = profileViewModel.Name,
            //    Surname = profileViewModel.Surname,
            //    Birthday = profileViewModel.Birthday,
            //    City = profileViewModel.City,
            //    PlaceOfStudy = profileViewModel.PlaceOfStudy,
            //    Skype = profileViewModel.Skype
            //};
            //UserService.Update(profileDto);
            ////Update Profile
            //UserDTO userDto = await UserService.FindById();
            return new EmptyResult();
        }
    }
}