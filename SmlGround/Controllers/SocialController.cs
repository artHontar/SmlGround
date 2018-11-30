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
        
        private readonly IUserService _userService; 

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
                profileDto = await _userService.FindProfileAsync(id, null);
            else
                profileDto = await _userService.FindProfileAsync(curentUserId, id);
            
            var profileViewModel = Mapper.Map<ProfileDTO, ProfileViewModel>(profileDto);
            profileViewModel.IsCurrentUserProfile =
                profileViewModel.Id.Equals(curentUserId) ? true : false;

            var currentUser = await _userService.FindProfileAsync(curentUserId,null);

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;
            return View(profileViewModel);
        }

        public async Task<ActionResult> Friends(string id)
        {
            id = id ?? HttpContext.User.Identity.GetUserId();
              
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriendsProfileAsync(id,null));

            var currentUser = await _userService.FindProfileAsync(HttpContext.User.Identity.GetUserId(), null);

            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;

            return View("Friends",profileViewModelList);
        }

        [NonDirectAccess]
        public async Task<ActionResult> FindFriends(string text)
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();

            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriendsProfileAsync(currentUserId, text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("FriendsList", profileViewModelList);
            }
            return Content("По вашему запросу ничего не найдено");
        }

        [HttpPost]
        public async Task<ActionResult> AddFriend(string id)
        {
            await _userService.AddFriendshipAsync(HttpContext.User.Identity.GetUserId(), id);
            return Json("success");
        }

        [HttpPost]
        public async Task<ActionResult> ApproveFriend(string id)
        {
            await _userService.UpdateFriendshipAsync(id, HttpContext.User.Identity.GetUserId(), FriendStatus.Friend);
            return Json("success");
        }
        
        [HttpPost]
        public async Task<ActionResult> RejectFriend(string id)
        {
            await _userService.RejectFriendshipAsync(HttpContext.User.Identity.GetUserId(), id);
            return Json("success");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFriend(string id)
        {
            await _userService.DeleteFriendshipAsync(HttpContext.User.Identity.GetUserId(), id);
            return Json("success");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();
            var profileDto = await _userService.FindProfileAsync(currentUserId, id);
            var editProfileViewModel = Mapper.Map<ProfileDTO, EditProfileViewModel>(profileDto);
            
            var currentUser = await _userService.FindProfileAsync(currentUserId,null);

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
                
                await _userService.UpdateProfileAsync(profileDto);
                TempData["Success"] = "Изменения сохранены";
            }
            else
                TempData["Success"] = "Не удалось сохранить изменения";
            return RedirectToAction("Profile", "Social");
        }

        [HttpPost]
        public async Task<ActionResult> EditAvatar(string id, HttpPostedFileBase uploadImage)
        {
            var profileDto = await _userService.FindProfileAsync(id,null);

            if (ModelState.IsValid && uploadImage != null)
            {
                CastBinaryFormatter binaryFormatter = new CastBinaryFormatter(uploadImage);
                
                profileDto.Avatar = binaryFormatter.Convert();
                await _userService.UpdateAvatarAsync(profileDto);
                return Json(profileDto.Avatar);
            }

            return new EmptyResult();
        }

        public async Task<ActionResult> People()
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfilesAsync(currentUserId,null));

            var currentUser = await _userService.FindProfileAsync(currentUserId,null);
            ViewBag.Name = currentUser.Name;
            ViewBag.Avatar = currentUser.Avatar;
            return View("People", profileViewModelList);
        }

        [NonDirectAccess]
        public async Task<ActionResult> FindPeople(string text)
        {
            var currentUserId = HttpContext.User.Identity.GetUserId();

            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfilesAsync(currentUserId,text));//new List<ProfileViewModel>();
            if (profileViewModelList.Count > 0)
            {
                return PartialView("UsersList", profileViewModelList);
            }
            return Content("По вашему запросу ничего не найдено");
        }        
    }
}