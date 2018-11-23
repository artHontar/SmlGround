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
        Task<string> Create(UserRegistrationDTO userDto);
        Task<OperationDetails> ConfirmEmail(string Token,string Email);
        Task<ClaimsIdentity> Authenticate(UserConfirmDTO userDto);
        Task<ClaimsIdentity> AutoAuthenticate(UserConfirmDTO userDto);
        ProfileDTO FindProfile(string Id);
        void Update(ProfileWithoutAvatarDTO profileDto);
        void UpdateAvatar(ProfileDTO profileDto);
        List<ProfileDTO> GetAllProfiles(string search);
        Task SetInitialData(UserRegistrationDTO adminDto, List<string> roles);
    }
}
