using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SmlGround.DataAccess.Models
{
    public class SocialDbContext : IdentityDbContext<User>
    {
        public SocialDbContext() : base("DbConnection")
        {
        }

        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Friend> Friends { get; set; }
    }
}
