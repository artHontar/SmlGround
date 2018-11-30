using AutoMapper;
using SmlGround.DataAccess.Models;
using SmlGround.DLL.DTO;
using SmlGround.Models;
using Profile = SmlGround.DataAccess.Models.Profile;
namespace SmlGround.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AllowNullCollections = true;
                //Profile
                cfg.CreateMap<ProfileDTO, ProfileViewModel>().ForMember(t => t.Avatar, opt => opt.MapFrom(t => t.Avatar == null ? null : t.Avatar));
                cfg.CreateMap<ProfileDTO, EditProfileViewModel>();
                cfg.CreateMap<EditProfileViewModel, ProfileDTO>();
                cfg.CreateMap<EditProfileViewModel, ProfileWithoutAvatarDTO>();
                cfg.CreateMap<ProfileWithoutAvatarDTO,Profile>();
                cfg.CreateMap<Profile, ProfileDTO>();
                cfg.CreateMap<ProfileDTO, Profile>();

                //User
                cfg.CreateMap<RegistrationModel, UserRegistrationDTO>().ForMember(t => t.Role, opt => opt.MapFrom((src) => "user"));

                //Friend
                cfg.CreateMap<Friend, FriendDTO>();
                cfg.CreateMap<FriendDTO, Friend>();


            });
        }
    }
}