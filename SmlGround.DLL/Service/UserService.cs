using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;
using SmlGround.DLL.Interfaces;

namespace SmlGround.DLL.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<string> Create(UserDTO userDto)
        {
            User user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new User{Email = userDto.Email,UserName = userDto.UserName ,RegistrationTime = DateTime.Now};
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if(result.Errors.Count() > 0)
                    return null;
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // create client's profile
                Profile profile = new Profile{Id = user.Id,Name = userDto.Name,Surname = userDto.Surname,Birthday = userDto.Birthday};
                Database.ClientManager.Create(profile);
                await Database.SaveAsync();
                return user.Id;
            }
            else
            {
                return "Пользователь с таким логином уже существует";
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            //find user
            User user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {
                claim = await Database.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }
        
        public User FindByEmail(string Email)
        {
            User user = Database.UserManager.FindByEmail(Email);
            
            return user;
        }
        
        public async Task<OperationDetails> ConfirmEmail(string Token,string Email)
        {
            User user = await Database.UserManager.FindByIdAsync(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.EmailConfirmed = true;
                    await Database.UserManager.UpdateAsync(user);
                    return new OperationDetails(true, "Регистрация успешно пройдена", "");
                }
                else
                {
                    return new OperationDetails(false, "Повторите", "");
                }
            }
            return new OperationDetails(false, "Подтвердить Email не удалось", "");
        }
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
