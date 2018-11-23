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

namespace SmlGround.DLL.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork database { get; set; }

        public UserService(IUnitOfWork unitOfWork)
        {
            database = unitOfWork;
        }
        public List<ProfileDTO> GetAllProfiles(string search)
        {
            List<Profile> list = database.ProfileManager.GetAllProfiles();
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
                database.ProfileManager.Create(profile);
                await database.SaveAsync();
                return user.Id;
            }
            return "Пользователь с таким логином уже существует";
        }

        public void Update(ProfileWithoutAvatarDTO profileDto)
        {
            var profile = database.ProfileManager.GetProfile(profileDto.Id);

            Mapper.Map(profileDto, profile);
            //profile.Name = profileDto.Name;
            //profile.Surname = profileDto.Surname;
            //profile.Birthday = profileDto.Birthday;
            //profile.City = profileDto.City;
            //profile.PlaceOfStudy = profileDto.PlaceOfStudy;
            //profile.Skype = profileDto.Skype;

            database.ProfileManager.Update(profile);
        }

        public void UpdateAvatar(ProfileDTO profileDto)
        {
            var profile = database.ProfileManager.GetProfile(profileDto.Id);
            profile.Avatar = profileDto.Avatar;
            
            if (profile != null)
            {
                database.ProfileManager.Update(profile);
            }
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

        public ProfileDTO FindProfile(string Id)
        {
            var profile = database.ProfileManager.GetProfile(Id);
            var userProfileDto = Mapper.Map<Profile,ProfileDTO>(profile);
            
            return userProfileDto;
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
