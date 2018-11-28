using AutoMapper;
using Microsoft.AspNet.Identity;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using Profile = SmlGround.DataAccess.Models.Profile;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;
using SmlGround.DLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Common.Enum;

namespace SmlGround.DLL.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork database { get; set; }

        public UserService(IUnitOfWork unitOfWork)
        {
            database = unitOfWork;
        }

        public async Task<UserDTO> GetUserById(string id)
        {
            var user = await database.UserManager.FindByIdAsync(id);
            var userDto = Mapper.Map<User,UserDTO>(user);
            return userDto; 
        }

        public async Task<IEnumerable<ProfileDTO>> GetAllProfiles(string search)
        {
            var list = await database.ProfileManager.GetAllAsync();
            List<ProfileDTO> listDto = list.Where(profile => string.IsNullOrEmpty(search) || profile.Name.Contains(search) || profile.Surname.Contains(search))
            .Select(item => Mapper.Map<Profile ,ProfileDTO>(item)).ToList();
            return listDto;
        }
        public async Task<string> Create(UserRegistrationDTO userDto)
        {
            var user = await database.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new User{Email = userDto.Email,UserName = userDto.UserName ,RegistrationTime = DateTime.Now};
                var result = await database.UserManager.CreateAsync(user, userDto.Password);
                if(result.Errors.Any())
                    return null;
                await database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                
                var profile = new Profile { Id = user.Id, Name = userDto.Name, Surname = userDto.Surname, Birthday = userDto.Birthday };
                //user.Profile = profile;
                //await database.UserManager.UpdateAsync(user);
                await database.ProfileManager.CreateAsync(profile);
                return user.Id;
            }
            return "Пользователь с таким логином уже существует";
        }

        public async Task<int> Update(ProfileWithoutAvatarDTO profileDto)
        {
            var profile = database.ProfileManager.Get(profileDto.Id);

            Mapper.Map(profileDto, profile);
            
            return await database.ProfileManager.UpdateAsync(profile);
        }

        public async Task UpdateAvatar(ProfileDTO profileDto)
        {
            var profile = database.ProfileManager.Get(profileDto.Id);
            profile.Avatar = profileDto.Avatar;

            await database.ProfileManager.UpdateAsync(profile);
        }

        public async Task AddFriend(string idBy,string idTo)
        {
            var friendship = new Friend
            {
                UserById = idBy,
                UserToId = idTo,
                CreationTime = DateTime.Now,
                FriendRequestFlag = FriendRequestFlag.None
            };
            await database.FriendManager.CreateAsync(friendship);
           
        }
        
        public async Task<IEnumerable<ProfileDTO>> GetAllFriends(string id)
        {
            var listFriends = await database.FriendManager.GetAllAsync();
            
            var user = listFriends.Where(o => o.FriendRequestFlag == FriendRequestFlag.None && o.UserById == id).Select(o => o.UserBy).FirstOrDefault();
                                       //o => (o.UserById == id) || (o.UserToId == id && o.FriendRequestFlag == FriendRequestFlag.Approved)
            var listUserFriends = user?.SentFriends.Select(o => o.UserTo.Profile).ToList();
            var listDto = listUserFriends?.Select(item => Mapper.Map<Profile, ProfileDTO>(item)).ToList();
            
            return listDto == null? new List<ProfileDTO>() : listDto;
        }

        //public async Task<IEnumerable<ProfileDTO>> GetAllApprovedFriends(string id)
        //{
        //    var listFriends = await database.FriendManager.GetAllAsync();

        //    var user = listFriends.Where(o => o.FriendRequestFlag == FriendRequestFlag. && o.UserById == id).Select(o => o.UserBy).FirstOrDefault();
        //    var listUserFriends = user?.SentFriends.Select(o => o.UserTo.Profile).ToList();
        //    var listDto = listUserFriends?.Select(item => Mapper.Map<Profile, ProfileDTO>(item)).ToList();

        //    return listDto == null ? new List<ProfileDTO>() : listDto;
        //}

        public async Task<ClaimsIdentity> Authenticate(UserConfirmDTO userDto)
        {
            ClaimsIdentity claim = null;

            var user = await database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {
                claim = await database.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task<ClaimsIdentity> AutoAuthenticate(UserConfirmDTO userDto)
        {
            ClaimsIdentity claim = null;

            var user = await database.UserManager.FindByEmailAsync(userDto.Email);
            if (user != null)
            {
                claim = await database.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public async Task<ProfileDTO> FindProfile(string Id)
        {
            var profile = await database.ProfileManager.GetAsync(Id);
            var userProfileDto = Mapper.Map<Profile,ProfileDTO>(profile);
            
            return userProfileDto;
        }

        public async Task AddFriend()
        {
            //var friendship = new Friend{ UserOneId = "",UserTwoId = "", CreationTime = DateTime.Now };
        }

        public async Task<OperationDetails> ConfirmEmail(string Token,string Email)
        {
            var user = await database.UserManager.FindByIdAsync(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.EmailConfirmed = true;
                    await database.UserManager.UpdateAsync(user);
                    return new OperationDetails(true, "Регистрация успешно пройдена", "");
                }
                return new OperationDetails(false, "Повторите", "");
            }
            return new OperationDetails(false, "Подтвердить Email не удалось", "");
        }

        public async Task SetInitialData(UserRegistrationDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
