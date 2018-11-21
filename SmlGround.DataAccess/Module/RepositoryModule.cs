using Autofac;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Repositories;

namespace SmlGround.DataAccess.Module
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProfileRepository>()
                .As<IProfileRepository>().InstancePerRequest();
            
        }
    }
}
