using System.Data.Entity;
using System.Web;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Module
{
    
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new SocialDbContext("DbConnection")).SingleInstance().InstancePerRequest();
            //builder.RegisterType<SocialDbContext>().As<DbContext>();
            builder.RegisterModule(new RepositoryModule());
            
        }

    }
}
