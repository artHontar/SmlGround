using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SmlGround.DLL.DTO;
using SmlGround.Models;

namespace SmlGround.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<ProfileDTO, ProfileViewModel>().ForMember(t => t.Avatar, opt => opt.ResolveUsing(t => t.Avatar == null ? null : t.Avatar));
                cfg.CreateMap<ProfileDTO, EditProfileViewModel>();
                cfg.CreateMap<EditProfileViewModel, ProfileDTO>();
            });
        }
    }
}