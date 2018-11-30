using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using SmlGround.DataAccess.Repositories;

namespace SmlGround.DataAccess.Module
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProfileRepository>()
                .As<IRepository<Profile,string>>().InstancePerRequest();

            builder.RegisterType<FriendRepository>()
                .As<IRepository<Friend,string[]>>().InstancePerRequest();

            //builder.RegisterType<UserStore<User>>().As<IUserStore<User>>();
            //builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>();

            builder.Register(c => new UserStore<User>(c.Resolve<SocialDbContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c => new RoleStore<IdentityRole>(c.Resolve<SocialDbContext>())).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
            
        }
    }
}
