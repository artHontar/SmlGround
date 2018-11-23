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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NLog;
using SmlGround.Filters;

namespace SmlGround.Controllers
{
    [Authorize]
    [LogInfo]
    [NullException]
    public class SocialController : Controller
    {
        
        private readonly IUserService _userService; //=> HttpContext.GetOwinContext().GetUserManager<IUserService>();

        //public ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        //private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public SocialController(IUserService userService)
        {
            this._userService = userService;
        }


        [ActionName("Profile")]
        
        public ActionResult UserProfile(string id)
        {
            id = id == null ? HttpContext.User.Identity.GetUserId() : id;
        
            ViewBag.Success = TempData["Success"]?.ToString();

            var profileDto = _userService.FindProfile(id);

            var profileViewModel = Mapper.Map<ProfileDTO, ProfileViewModel>(profileDto);
        
            profileViewModel.IsCurrentUserProfile =
                profileViewModel.Id.Equals(HttpContext.User.Identity.GetUserId()) ? true : false;
            

            return View(profileViewModel);
        }

        public ActionResult Edit(string id)
        {
            var profileDto = _userService.FindProfile(id);

            var editProfileViewModel = Mapper.Map<ProfileDTO, EditProfileViewModel>(profileDto);

            return View("Edit",editProfileViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Birthday,City,PlaceOfStudy")]EditProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                var profileDto = Mapper.Map<EditProfileViewModel, ProfileWithoutAvatarDTO>(profileViewModel);
                
                _userService.Update(profileDto);
                TempData["Success"] = "Изменения сохранены";
            }
            else
                TempData["Success"] = "Не удалось сохранить изменения";
            return RedirectToAction("Profile", "Social");
        }

        [HttpPost]
        public ActionResult EditAvatar(string id, HttpPostedFileBase uploadImage)
        {
            ProfileDTO profileDto = _userService.FindProfile(id);

            if (ModelState.IsValid && uploadImage != null)
            {
                CastBinaryFormatter binaryFormatter = new CastBinaryFormatter(uploadImage);
                
                profileDto.Avatar = binaryFormatter.Convert();
                _userService.UpdateAvatar(profileDto);
                return Json(profileDto.Avatar);
            }

            return new EmptyResult();
        }

        public ActionResult People()
        {
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(_userService.GetAllProfiles(null));//new List<ProfileViewModel>();
            
            return View("People", profileViewModelList);
        }

        [NonDirectAccess]
        public ActionResult FindPeople(string text)
        {
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(_userService.GetAllProfiles(text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("UsersList", profileViewModelList);
            }
            return Content("По вашему запросу ничего не найдено");
        }
    }
}