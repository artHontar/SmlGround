using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Models;
using SmlGround.DataAccess.Module;
using SmlGround.DataAccess.Repositories;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;

namespace SmlGround.Util
{
    class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            
            // Run other optional steps, like registering model binders,
            // web abstractions, etc., then set the dependency resolver
            // to be Autofac.
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<UserService>()
                .As<IUserService>().InstancePerRequest();

            builder.RegisterModule(new ServiceModule());

            builder.RegisterModule(new EFModule());

            
            builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>();

            builder.Register(c => new UserStore<User>(c.Resolve<SocialDbContext>())).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<ApplicationRoleManager>();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
            {
                DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("SmlGround​")
            });
            
            
            


            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}