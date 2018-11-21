using Microsoft.AspNet.Identity;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
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
            .Select(item => new ProfileDTO
            {
                Id = item.Id,
                Name = item.Name,
                Surname = item.Surname,
                Avatar = item.Avatar,
                Birthday = item.Birthday,
                City = item.City,
                PlaceOfStudy = item.PlaceOfStudy,
                Skype = item.Skype
            }).ToList();
           
            return listDto;
        }
        public async Task<string> Create(UserDTO userDto)
        {
            var user = await database.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new User{Email = userDto.Email,UserName = userDto.UserName ,RegistrationTime = DateTime.Now};
                var result = await database.UserManager.CreateAsync(user, userDto.Password);
                if(result.Errors.Any())
                    return null;
                await database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // create client's profile
                var profile = new Profile { Id = user.Id, Name = userDto.Name, Surname = userDto.Surname, Birthday = userDto.Birthday };
                database.ProfileManager.Create(profile);
                database.Save();
                return user.Id;
            }
            return "Пользователь с таким логином уже существует";
        }

        public void Update(ProfileDTO profileDto)
        {
            var profile = database.ProfileManager.GetProfile(profileDto.Id);

            profile.Name = profileDto.Name;
            profile.Surname = profileDto.Surname;
            profile.Birthday = profileDto.Birthday;
            profile.City = profileDto.City;
            profile.PlaceOfStudy = profileDto.PlaceOfStudy;
            profile.Skype = profileDto.Skype;
            
            database.ProfileManager.Update(profile);
            database.Save();
            
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

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
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

        public async Task<ClaimsIdentity> AutoAuthenticate(UserDTO userDto)
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

        //public UserDTO FindByEmail(string Email)
        //{
        //    User user = Database.UserManager.FindByEmail(Email);
        //    UserDTO userDto = new UserDTO {Id = user.Id,Email = user.Email, Password = user.PasswordHash, UserName = user.UserName};
        //    return userDto;
        //}

        public ProfileDTO FindProfile(string Id)
        {
            var profile = database.ProfileManager.GetProfile(Id);
            var userProfileDto = new ProfileDTO {Id = profile.Id, Avatar = profile.Avatar, Name = profile.Name, Surname = profile.Surname, Birthday = profile.Birthday,
                                                      City = profile.City, PlaceOfStudy = profile.PlaceOfStudy, Skype = profile.Skype};
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

        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
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
