using Microsoft.AspNet.Identity.EntityFramework;
using SmlGround.DataAccess.EF;
using SmlGround.DataAccess.Identity;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private SocialDbContext db { get; }
        private ApplicationUserManager userManager { get; }
        private ApplicationRoleManager roleManager { get; }
        private IProfileRepository profileManager { get; }

        public IdentityUnitOfWork(SocialDbContext db, ApplicationUserManager userManager, ApplicationRoleManager roleManager,  IProfileRepository profileManager) 
         {
            this.db = db;
             this.userManager = userManager; //new ApplicationUserManager(new UserStore<User>(db));
             this.roleManager = roleManager; //new ApplicationRoleManager(new RoleStore<IdentityRole>(db));
            this.profileManager = profileManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }
        
        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public IProfileRepository ProfileManager
        {
            get { return profileManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    profileManager.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
