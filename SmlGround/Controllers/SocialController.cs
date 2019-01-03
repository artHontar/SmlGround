using System;
using AutoMapper;
using Common.Enum;
using Microsoft.AspNet.Identity;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;
using SmlGround.Filters;
using SmlGround.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;

namespace SmlGround.Controllers
{
    class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
    }

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
            var currentUserId = HttpContext.User.Identity.GetUserId();
            ViewBag.Success = TempData["Success"]?.ToString();
            ProfileDTO profileDto;
            if (id == currentUserId)
                profileDto = await _userService.FindProfileAsync(id, null);
            else
                profileDto = await _userService.FindProfileAsync(currentUserId, id);
            
            var profileViewModel = Mapper.Map<ProfileDTO, ProfileViewModel>(profileDto);
            profileViewModel.IsCurrentUserProfile =
                profileViewModel.Id.Equals(currentUserId) ? true : false;

            if (Session["name"] == null || Session["avatar"] == null)
            {
                var currentUser = await _userService.FindProfileAsync(currentUserId, null);
                Session["name"] = currentUser.Name;
                Session["avatar"] = currentUser.Avatar;
            }
            ViewBag.Name = Session["name"];
            ViewBag.Avatar = Session["avatar"];
            //using (var client = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:9091/api/values/4"))
            //    {
            //        StringContent queryString = new StringContent("{\"Id\":\"4 \",\"Name\":\"rew\",\"Author\":\"qwe\",\"Year\":\"1999\"}", Encoding.UTF8, "application/json");

            //        request.Content = queryString;
                    

            //        using (var response = await client.SendAsync(request))
            //        {
            //            var result = response.Content.ReadAsStringAsync();
            //        }
            //    }
                
                
            //    //GET
            //    //var _response = await client.GetAsync("http://localhost:9091/api/values/");
            //    //var result = await _response.Content.ReadAsStringAsync();
            //    //Book[] books = JsonConvert.DeserializeObject<Book[]>(result);
            //    //ViewBag.Api = result;
            //}

            return View(profileViewModel);
        }

        public async Task<ActionResult> Dialog()
        {
            return View();
        }

        public async Task<ActionResult> Friends(string id)
        {
            id = id ?? HttpContext.User.Identity.GetUserId();
              
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllFriendsProfileAsync(id,null));

            if (Session["name"] == null || Session["avatar"] == null)
            {
                var currentUser = await _userService.FindProfileAsync(id, null);
                Session["name"] = currentUser.Name;
                Session["avatar"] = currentUser.Avatar;
            }
            ViewBag.Name = Session["name"];
            ViewBag.Avatar = Session["avatar"];

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

            if (Session["name"] == null || Session["avatar"] == null)
            {
                var currentUser = await _userService.FindProfileAsync(currentUserId, null);
                Session["name"] = currentUser.Name;
                Session["avatar"] = currentUser.Avatar;
            }
            ViewBag.Name = Session["name"];
            ViewBag.Avatar = Session["avatar"];
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
            Session.Clear();
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
                Session.Clear();
                return Json(profileDto.Avatar);
            }

            return new EmptyResult();
        }
        
        [OutputCache(Duration = 30,Location = OutputCacheLocation.Client)]
        public async Task<ActionResult> People()
        {
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(30));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            var currentUserId = HttpContext.User.Identity.GetUserId();
            var profileViewModelList = Mapper.Map<IEnumerable<ProfileDTO>, List<ProfileViewModel>>(await _userService.GetAllProfilesAsync(currentUserId,null));

            if (Session["name"] == null || Session["avatar"] == null)
            {
                var currentUser = await _userService.FindProfileAsync(currentUserId, null);
                Session["name"] = currentUser.Name;
                Session["avatar"] = currentUser.Avatar;
            }
            ViewBag.Name = Session["name"];
            ViewBag.Avatar = Session["avatar"];
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