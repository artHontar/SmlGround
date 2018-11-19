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
        private IClientManager clientManager { get; }

        public IdentityUnitOfWork(string connectionString)
        {
            db = new SocialDbContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<User>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }
        
        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
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

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
