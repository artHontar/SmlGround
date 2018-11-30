using System;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using Profile = SmlGround.DataAccess.Models.Profile;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;
using SmlGround.DLL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<ProfileDTO>> GetAllProfiles(string id,string search)
        {
            var profileList = Mapper.Map<IEnumerable<Profile>,List<ProfileDTO>>(await database.ProfileManager.GetAllAsync());
            var list = await GetAllPeopleWithStatus(id, profileList);
            var listDto = list.Where(profile => string.IsNullOrEmpty(search) 
                                                || profile.Name.Contains(search)
                                                || profile.Surname.Contains(search))
            .ToList();
            return listDto;
        }

        public async Task<IEnumerable<ProfileDTO>> GetAllFriendsProfile(string id, string search)
        {
            var profileList = Mapper.Map<IEnumerable<Profile>, List<ProfileDTO>>(await database.ProfileManager.GetAllAsync());
            var list = await GetAllApprovedFriends(id);
            var listDto = list.Where(profile => string.IsNullOrEmpty(search)
                                                || profile.Name.Contains(search)
                                                || profile.Surname.Contains(search))
                .ToList();
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
                await database.ProfileManager.CreateAsync(profile);
                return user.Id;
            }
            return "Пользователь с таким логином уже существует";
        }

        public async Task<int> UpdateProfile(ProfileWithoutAvatarDTO profileDto)
        {
            var profile = database.ProfileManager.Get(profileDto.Id);
            
            //update fields on profile
            Mapper.Map(profileDto, profile);
            
            return await database.ProfileManager.UpdateAsync(profile);
        }

        public async Task UpdateAvatar(ProfileDTO profileDto)
        {
            var profile = database.ProfileManager.Get(profileDto.Id);
            profile.Avatar = profileDto.Avatar;

            await database.ProfileManager.UpdateAsync(profile);
        }

        public async Task AddFriendship(string idBy,string idTo)
        {
            var friendship = new Friend
            {
                UserById = idBy,
                UserToId = idTo,
                CreationTime = DateTime.Now,
                FriendRequestFlag = FriendStatus.Signed
            };
            await database.FriendManager.CreateAsync(friendship);  
        }

        public async Task<int> UpdateFriendship(string idBy, string idTo, FriendStatus operation)
        {
            var friendship = await database.FriendManager.GetAsync(new[] { idBy, idTo }) ?? await database.FriendManager.GetAsync(new[] { idTo, idBy });
            friendship.FriendRequestFlag = operation;

            return await database.FriendManager.UpdateAsync(friendship);
        }

        public async Task<int> DeleteFriendship(string idBy, string idTo)
        {
            var friendship = await database.FriendManager.GetAsync(new[] { idBy, idTo });
            if (friendship != null)
                friendship.FriendRequestFlag = FriendStatus.Subscriber;
            else
            {
                friendship = await database.FriendManager.GetAsync(new[] {idTo, idBy});
                friendship.FriendRequestFlag = FriendStatus.Signed;
            }
            
            return await database.FriendManager.UpdateAsync(friendship);
        }

        public async Task RejectFriendship(string idBy, string idTo)
        {
            var friendship = await database.FriendManager.GetAsync(new[] { idBy, idTo }) ?? await database.FriendManager.GetAsync(new[] { idTo, idBy });
            
            await database.FriendManager.DeleteAsync(new [] { friendship.UserById,friendship.UserToId });
        }

        private async Task<IEnumerable<ProfileDTO>> GetAllSubscribersFriends(string id)
        {
            var listFriends = (await database.FriendManager.GetAllAsync()).ToList();
            
            var user = listFriends.Where(o => o.UserById == id && (o.FriendRequestFlag == FriendStatus.Signed)).Select(o => o.UserBy).FirstOrDefault();
            var listUserFriends1 = user?.SentFriends.Where(o => o.FriendRequestFlag == FriendStatus.Signed).Select(o => o.UserTo.Profile).ToList();

            user = listFriends.Where(o => o.UserToId == id && (o.FriendRequestFlag == FriendStatus.Subscriber)).Select(o => o.UserTo).FirstOrDefault();
            var listUserFriends2 = user?.ReceievedFriends.Where(o => o.FriendRequestFlag == FriendStatus.Subscriber).Select(o => o.UserBy.Profile).ToList();
            var list = listUserFriends1?.Concat(listUserFriends2 ?? new List<Profile>()) ?? listUserFriends2;
            var listDto = list?.Select(item => Mapper.Map<Profile, ProfileDTO>(item)).ToList();
            
            return listDto == null? new List<ProfileDTO>() : listDto;
        }
        private async Task<IEnumerable<ProfileDTO>> GetAllApprovedFriends(string id)
        {
            var listFriends = (await database.FriendManager.GetAllAsync()).ToList();

            var user = listFriends.Where(o => o.UserToId == id).Select(o => o.UserTo).FirstOrDefault();
            var listUserFriends1 = user?.ReceievedFriends.Where(o => o.FriendRequestFlag == FriendStatus.Friend).Select(o => o.UserBy.Profile).ToList();

            user = listFriends.Where(o => o.UserById == id).Select(o => o.UserBy).FirstOrDefault();
            var listUserFriends2 = user?.SentFriends.Where(o => o.FriendRequestFlag == FriendStatus.Friend).Select(o => o.UserTo.Profile).ToList();
            var listUserFriends = listUserFriends1 == null ? listUserFriends2 : listUserFriends1.Union(listUserFriends2 == null ? new List<Profile>() : listUserFriends2);
            var listDto = listUserFriends?.Select(item => Mapper.Map<Profile, ProfileDTO>(item)).ToList();

            return listDto == null ? new List<ProfileDTO>() : listDto;
        }

        private async Task<IEnumerable<ProfileDTO>> GetAllSignersFriends(string id)
        {
            var listFriends = (await database.FriendManager.GetAllAsync()).ToList();

            var user = listFriends.Where(o => o.UserToId == id).Select(o => o.UserTo).FirstOrDefault();
            var listUserFriends1 = user?.ReceievedFriends.Where(o => o.FriendRequestFlag == FriendStatus.Signed).Select(o => o.UserBy.Profile).ToList();

            user = listFriends.Where(o => o.UserById == id).Select(o => o.UserBy).FirstOrDefault();
            var listUserFriends2 = user?.SentFriends.Where(o => o.FriendRequestFlag == FriendStatus.Subscriber).Select(o => o.UserTo.Profile).ToList();
            var list = listUserFriends1?.Concat(listUserFriends2 ?? new List<Profile>()) ?? listUserFriends2;
            var listDto = list?.Select(item => Mapper.Map<Profile, ProfileDTO>(item)).ToList();

            return listDto == null ? new List<ProfileDTO>() : listDto;
        }

        private async Task<IEnumerable<ProfileDTO>> GetAllPeopleWithStatus(string id, IEnumerable<ProfileDTO> profileDto)
        {
            var sentFriends = (await GetAllSubscribersFriends(id)).ToList();
            var list = profileDto.Except(sentFriends).ToList();
            foreach (var item in sentFriends)
            {
                item.FriendFlag = FriendStatus.Subscriber;
            }
            profileDto = list.Union(sentFriends).ToList();

            var nonApprovedfriends = (await GetAllSignersFriends(id)).ToList();
            list = profileDto.Except(nonApprovedfriends).ToList();
            foreach (var item in nonApprovedfriends)
            {
                item.FriendFlag = FriendStatus.Signed;
            }
            profileDto = list.Union(nonApprovedfriends).ToList();

            var approvedfriends = (await GetAllApprovedFriends(id)).ToList();
            list = profileDto.Except(approvedfriends).ToList();
            foreach (var item in approvedfriends)
            {
                item.FriendFlag = FriendStatus.Friend;
            }
            profileDto = list.Union(approvedfriends).ToList();

            var currentUser = await FindProfile(id, null);

            var profileDtoList = profileDto.ToList();

            profileDtoList.Remove(currentUser);
            
            return profileDtoList;
        }

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

        public async Task<ProfileDTO> FindProfile(string idCurrent, string id)
        {
            Profile profile;
            ProfileDTO userProfileDto;
            if (id == null)
            {
                profile = await database.ProfileManager.GetAsync(idCurrent);
                userProfileDto = Mapper.Map<Profile, ProfileDTO>(profile);
            }
            else
            {
                profile = await database.ProfileManager.GetAsync(id);
                userProfileDto = Mapper.Map<Profile, ProfileDTO>(profile);

                var friendship = await database.FriendManager.GetAsync(new[] { idCurrent, id });
                if (friendship != null)
                {
                    if (friendship.FriendRequestFlag == FriendStatus.Subscriber)
                        userProfileDto.FriendFlag = FriendStatus.Signed;
                    else if (friendship.FriendRequestFlag == FriendStatus.Signed)
                        userProfileDto.FriendFlag = FriendStatus.Subscriber;
                    else
                        userProfileDto.FriendFlag = friendship.FriendRequestFlag;
                }
                else
                {
                    friendship = await database.FriendManager.GetAsync(new[] { id, idCurrent });
                    if(friendship != null)
                        userProfileDto.FriendFlag = friendship.FriendRequestFlag;
                }
            }
            return userProfileDto;
            
        }
        
        public async Task<OperationDetails> ConfirmEmail(string token,string email)
        {
            var user = await database.UserManager.FindByIdAsync(token);
            if (user != null)
            {
                if (user.Email == email)
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
