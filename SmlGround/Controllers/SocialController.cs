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
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NLog;
using SmlGround.DataAccess.Models;
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
        public async Task<ActionResult> UserProfile(string id)
        {
            id = id == null ? HttpContext.User.Identity.GetUserId() : id;
        
            ViewBag.Success = TempData["Success"]?.ToString();

            var profileDto = await _userService.FindProfile(id);

            var profileViewModel = Mapper.Map<ProfileDTO, ProfileViewModel>(profileDto);
        
            profileViewModel.IsCurrentUserProfile =
                profileViewModel.Id.Equals(HttpContext.User.Identity.GetUserId()) ? true : false;

            var currentUser = await _userService.FindProfile(HttpContext.User.Identity.GetUserId());

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;
            return View(profileViewModel);
        }

        public async Task<ActionResult> Friends(string id)
        {
            id = id == null ? HttpContext.User.Identity.GetUserId() : id;

            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriends(id));

            var currentUser = await _userService.FindProfile(HttpContext.User.Identity.GetUserId());

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;

            return View("Friends",profileViewModelList);
        }

        [HttpPost]
        public async Task<ActionResult> AddFriend(string id)
        {
            await _userService.AddFriend(HttpContext.User.Identity.GetUserId(), id);
            return new EmptyResult();
        }

        public async Task<ActionResult> Edit(string id)
        {
            var profileDto = await _userService.FindProfile(id);
            var editProfileViewModel = Mapper.Map<ProfileDTO, EditProfileViewModel>(profileDto);
            
            var currentUser = await _userService.FindProfile(HttpContext.User.Identity.GetUserId());

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;

            return View("Edit",editProfileViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Birthday,City,PlaceOfStudy")]EditProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                var profileDto = Mapper.Map<EditProfileViewModel, ProfileWithoutAvatarDTO>(profileViewModel);
                
                await _userService.Update(profileDto);
                TempData["Success"] = "Изменения сохранены";
            }
            else
                TempData["Success"] = "Не удалось сохранить изменения";
            return RedirectToAction("Profile", "Social");
        }

        [HttpPost]
        public async Task<ActionResult> EditAvatar(string id, HttpPostedFileBase uploadImage)
        {
            var profileDto = await _userService.FindProfile(id);

            if (ModelState.IsValid && uploadImage != null)
            {
                CastBinaryFormatter binaryFormatter = new CastBinaryFormatter(uploadImage);
                
                profileDto.Avatar = binaryFormatter.Convert();
                await _userService.UpdateAvatar(profileDto);
                return Json(profileDto.Avatar);
            }

            return new EmptyResult();
        }

        public async Task<ActionResult> People()
        {
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfiles(null));//new List<ProfileViewModel>();
            var currentUserId = HttpContext.User.Identity.GetUserId();
            var friends = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriends(currentUserId));
            var list = profileViewModelList.Except(friends).ToList();
            foreach (var item in friends)
            {
                item.IsFriend = true;
            }
            profileViewModelList = list.Union(friends).ToList();
            //Во View отображение друг не друг
            var currentUser = await _userService.FindProfile(currentUserId);
            profileViewModelList.Remove(Mapper.Map<ProfileDTO, ProfileViewModel>(currentUser));
            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;
            return View("People", profileViewModelList);
        }

        [NonDirectAccess]
        public async Task<ActionResult> FindPeople(string text)
        {
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfiles(text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("UsersList", profileViewModelList);
            }
            return Content("По вашему запросу ничего не найдено");
        }
        
        //public async Task<ActionResult> Friends()
        //{
        //    var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfiles(null));//new List<ProfileViewModel>();

        //    var currentUser = await _userService.FindProfile(HttpContext.User.Identity.GetUserId());

        //    ViewBag.Name = currentUser.Name;
        //    ViewBag.Avatar = currentUser.Avatar;
        //    return View("Friends", profileViewModelList);
        //}
    }
}