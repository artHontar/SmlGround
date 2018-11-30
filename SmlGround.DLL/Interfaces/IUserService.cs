using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
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
        Task<ProfileDTO> FindProfile(string idCurrent,string id);
        Task<int> UpdateProfile(ProfileWithoutAvatarDTO profileDto);
        Task UpdateAvatar(ProfileDTO profileDto);
        Task AddFriendship(string idBy, string idTo);
        Task<int> UpdateFriendship(string idBy, string idTo, FriendStatus operation);
        Task RejectFriendship(string idBy, string idTo);
        Task<int> DeleteFriendship(string idBy, string idTo);
        Task<IEnumerable<ProfileDTO>> GetAllProfiles(string id, string search);
        Task<IEnumerable<ProfileDTO>> GetAllFriendsProfile(string id, string search);
        Task SetInitialData(UserRegistrationDTO adminDto, List<string> roles);
    }
}
