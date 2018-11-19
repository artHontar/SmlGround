using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.Identity;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;
using SmlGround.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace SmlGround.Controllers
{
    [Authorize]
    public class SocialController : Controller
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        public ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        
        [ActionName("Profile")]
        public ActionResult UserProfile(string id)
        {
            //UserDTO userDto = await UserService.FindById();
            if (id == null)
            {
                id = HttpContext.User.Identity.GetUserId();
                ViewBag.Success = TempData["Success"]?.ToString();
            }

            var profileDto = UserService.FindProfile(id);

            var profileViewModel = Mapper.Map<ProfileDTO, ProfileViewModel>(profileDto);
            
            if (profileViewModel.Id == HttpContext.User.Identity.GetUserId())
            {
                profileViewModel.IsCurrentUserProfile = true;
            }
            return View(profileViewModel);
        }

        public ActionResult Edit(string id)
        {
            var profileDto = UserService.FindProfile(id);

            var editProfileViewModel = Mapper.Map<ProfileDTO, EditProfileViewModel>(profileDto);

            return View("Edit",editProfileViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Birthday,City,PlaceOfStudy")]EditProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                var profileDto = Mapper.Map<EditProfileViewModel, ProfileDTO>(profileViewModel);
                
                UserService.Update(profileDto);
                TempData["Success"] = "Изменения сохранены";
            }
            else
            {
                TempData["Success"] = "Не удалось сохранить изменения";
            }

            //Update Profile
            //UserDTO userDto = await UserService.FindById();
            return RedirectToAction("Profile", "Social");
        }

        [HttpPost]
        public ActionResult EditAvatar(string id, HttpPostedFileBase uploadImage)
        {
            ProfileDTO profileDto = UserService.FindProfile(id);

            if (ModelState.IsValid&& uploadImage != null)
            {
                CastBinaryFormatter binaryFormatter = new CastBinaryFormatter(uploadImage);
                
                profileDto.Avatar = binaryFormatter.Convert();
                UserService.UpdateAvatar(profileDto);
                return Json(profileDto.Avatar);
            }

            return new EmptyResult();
        }

        public ActionResult People()
        {
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(UserService.GetAllProfiles(null));//new List<ProfileViewModel>();
            
            return View("People", profileViewModelList);
        } 

        public ActionResult FindPeople(string text)
        {
            //var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(UserService.GetAllProfiles());//new List<ProfileViewModel>();
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(UserService.GetAllProfiles(text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("UsersList", profileViewModelList);
            }

            return Content("По вашему запросу ничего не найдено");
        }
    }
}