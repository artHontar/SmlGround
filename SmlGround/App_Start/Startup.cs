using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SmlGround.DLL.Interfaces;
using SmlGround.DLL.Service;

[assembly: OwinStartupAttribute(typeof(SmlGround.App_Start.Startup))]

namespace SmlGround.App_Start
{
    public class Startup
    {
        // Create service via factory of services
        IServiceCreator serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            // Initialize service via OWIN
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService("DbConnection");
        }
    }
}