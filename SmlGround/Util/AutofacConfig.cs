using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Models;
using SmlGround.DataAccess.Module;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;
using System.Web;
using System.Web.Mvc;

namespace SmlGround.Util
{
    class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<UserService>()
                .As<IUserService>().SingleInstance().InstancePerRequest();

            builder.RegisterModule(new ServiceModule());

            builder.RegisterModule(new EFModule());
            
            
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
            {
                DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("SmlGround​")
            });

            builder.Register(c => new IdentityFactoryOptions<ApplicationRoleManager>
            {
                DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("SmlGround​")
            });




            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}