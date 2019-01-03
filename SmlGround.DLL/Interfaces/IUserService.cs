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
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<string> CreateAsync(UserRegistrationDTO userDto);
        Task<OperationDetails> ConfirmEmailAsync(string Token,string Email);
        Task<ClaimsIdentity> AuthenticateAsync(UserConfirmDTO userDto);
        Task<ClaimsIdentity> AutoAuthenticateAsync(UserConfirmDTO userDto);
        Task<ProfileDTO> FindProfileAsync(string idCurrent,string id);
        Task<int> UpdateProfileAsync(ProfileWithoutAvatarDTO profileDto);
        Task UpdateAvatarAsync(ProfileDTO profileDto);
        Task AddFriendshipAsync(string idBy, string idTo);
        Task<int> UpdateFriendshipAsync(string idBy, string idTo, FriendStatus operation);
        Task RejectFriendshipAsync(string idBy, string idTo);
        Task<int> DeleteFriendshipAsync(string idBy, string idTo);
        Task<IEnumerable<ProfileDTO>> GetAllProfilesAsync(string id, string search);
        Task<IEnumerable<ProfileDTO>> GetAllFriendsProfileAsync(string id, string search);
        Task SetInitialDataAsync(UserRegistrationDTO adminDto, List<string> roles);
        Task<MessageDTO> CreateMessageAsync(string senderId, string receiverId, string text);
        Task<IEnumerable<MessageDTO>> GetMessagesAsync(string current, string id);
    }
}
