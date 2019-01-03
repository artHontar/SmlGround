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
        private readonly SocialDbContext db;

        public IdentityUnitOfWork(SocialDbContext db, ApplicationUserManager userManager, ApplicationRoleManager roleManager, IRepository<Profile,string> profileManager, IRepository<Friend,string[]> friendManager, IRepository<Message, long> messageManager, IRepository<Dialog, long> dialogManager) 
         {
            this.db = db;
            this.UserManager = userManager; //new ApplicationUserManager(new UserStore<User>(db));
            this.RoleManager = roleManager; //new ApplicationRoleManager(new RoleStore<IdentityRole>(db));
            this.ProfileManager = profileManager;
            this.FriendManager = friendManager;
            this.DialogManager = dialogManager;
            this.MessageManager = messageManager;
         }

        public ApplicationUserManager UserManager { get; }

        public ApplicationRoleManager RoleManager { get; }

        public IRepository<Profile, string> ProfileManager { get; }

        public IRepository<Friend, string[]> FriendManager { get; }

        public IRepository<Message, Int64> MessageManager { get; }
        public IRepository<Dialog, Int64> DialogManager { get; }

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
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    ProfileManager.Dispose();
                    FriendManager.Dispose();
                    DialogManager.Dispose();
                    MessageManager.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
