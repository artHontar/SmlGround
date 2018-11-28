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
        Task<UserDTO> GetUserById(string id);
        Task<string> Create(UserRegistrationDTO userDto);
        Task<OperationDetails> ConfirmEmail(string Token,string Email);
        Task<ClaimsIdentity> Authenticate(UserConfirmDTO userDto);
        Task<ClaimsIdentity> AutoAuthenticate(UserConfirmDTO userDto);
        Task<ProfileDTO> FindProfile(string Id);
        Task<int> Update(ProfileWithoutAvatarDTO profileDto);
        Task UpdateAvatar(ProfileDTO profileDto);
        Task AddFriend(string idBy, string idTo);
        Task<IEnumerable<ProfileDTO>> GetAllProfiles(string search);
        Task<IEnumerable<ProfileDTO>> GetAllFriends(string id);
        Task SetInitialData(UserRegistrationDTO adminDto, List<string> roles);
    }
}
