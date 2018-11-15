using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.Models;
using SmlGround.DLL.DTO;
using SmlGround.DLL.Infrastructure;

namespace SmlGround.DLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<string> Create(UserDTO userDto);
        Task<OperationDetails> ConfirmEmail(string Token,string Email);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task<ClaimsIdentity> AutoAuthenticate(UserDTO userDto);
        ProfileDTO FindProfile(string Id);
        void Update(ProfileDTO profileDto);
        void UpdateAvatar(ProfileDTO profileDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
    }
}
