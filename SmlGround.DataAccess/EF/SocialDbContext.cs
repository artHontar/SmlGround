using Microsoft.AspNet.Identity.EntityFramework;
using SmlGround.DataAccess.Configuration;
using SmlGround.DataAccess.Models;
using System.Data.Entity;

namespace SmlGround.DataAccess.EF
{
    public class SocialDbContext : IdentityDbContext<User>
    {
        public SocialDbContext() : base("name=DbConnection")
        {
        }
        public SocialDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
         {
            base.OnModelCreating(builder);
            builder.Configurations.Add(new UserConfiguration());
            builder.Configurations.Add(new MessageConfiguration());
            builder.Configurations.Add(new ProfileConfiguration());
            builder.Configurations.Add(new DialogConfiguration());
            builder.Configurations.Add(new PostConfiguration());
            builder.Configurations.Add(new FriendConfiguration());  
        }
    }
}
