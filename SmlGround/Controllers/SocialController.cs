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
using Common.Enum;
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
            var curentUserId = HttpContext.User.Identity.GetUserId();
            ViewBag.Success = TempData["Success"]?.ToString();
            ProfileDTO profileDto;
            if (id == curentUserId)
                profileDto = await _userService.FindProfile(id, null);
            else
                profileDto = await _userService.FindProfile(curentUserId, id);
            
            var profileViewModel = Mapper.Map<ProfileDTO, ProfileViewModel>(profileDto);
            profileViewModel.IsCurrentUserProfile =
                profileViewModel.Id.Equals(curentUserId) ? true : false;

            var currentUser = await _userService.FindProfile(curentUserId,null);

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;
            return View(profileViewModel);
        }

        public async Task<ActionResult> Friends(string id)
        {
            id = id == null ? HttpContext.User.Identity.GetUserId() : id;

            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriendsProfile(id,null));

            var currentUser = await _userService.FindProfile(HttpContext.User.Identity.GetUserId(), null);

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;

            return View("Friends",profileViewModelList);
        }

        [NonDirectAccess]
        public async Task<ActionResult> FindFriends(string text)
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();

            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriendsProfile(currentUserId, text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("FriendsList", profileViewModelList);
            }
            return Content("По вашему запросу ничего не найдено");
        }

        [HttpPost]
        public async Task<ActionResult> AddFriend(string id)
        {
            await _userService.AddFriendship(HttpContext.User.Identity.GetUserId(), id);
            return Json("success");
        }

        [HttpPost]
        public async Task<ActionResult> ApproveFriend(string id)
        {
            await _userService.UpdateFriendship(id, HttpContext.User.Identity.GetUserId(), FriendStatus.Friend);
            return Json("success");
        }

        //[HttpPost]
        //public async Task<ActionResult> ReturnFriend(string id)
        //{
        //    return Json("success");
        //}

        [HttpPost]
        public async Task<ActionResult> RejectFriend(string id)
        {
            await _userService.RejectFriendship(HttpContext.User.Identity.GetUserId(), id);
            return Json("success");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFriend(string id)
        {
            await _userService.DeleteFriendship(HttpContext.User.Identity.GetUserId(), id);
            return Json("success");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();
            var profileDto = await _userService.FindProfile(currentUserId, id);
            var editProfileViewModel = Mapper.Map<ProfileDTO, EditProfileViewModel>(profileDto);
            
            var currentUser = await _userService.FindProfile(currentUserId,null);

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
                
                await _userService.UpdateProfile(profileDto);
                TempData["Success"] = "Изменения сохранены";
            }
            else
                TempData["Success"] = "Не удалось сохранить изменения";
            return RedirectToAction("Profile", "Social");
        }

        [HttpPost]
        public async Task<ActionResult> EditAvatar(string id, HttpPostedFileBase uploadImage)
        {
            var profileDto = await _userService.FindProfile(id,null);

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
            var currentUserId = HttpContext.User.Identity.GetUserId();
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfiles(currentUserId,null));

            var currentUser = await _userService.FindProfile(currentUserId,null);
            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;
            return View("People", profileViewModelList);
        }

        [NonDirectAccess]
        public async Task<ActionResult> FindPeople(string text)
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();

            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfiles(currentUserId,text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("UsersList", profileViewModelList);
            }
            return Content("По вашему запросу ничего не найдено");
        }        
    }
}