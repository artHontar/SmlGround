using SmlGround.DataAccess.Models;
using System.Data.Entity.ModelConfiguration;

namespace SmlGround.DataAccess.Configuration
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users")
                .HasKey(o => o.Id);
            Property(o => o.RegistrationTime)
                .IsRequired();
            Property(o => o.UserName)
                .IsOptional();
            HasRequired(c => c.Profile)
                .WithRequiredPrincipal(o => o.User);
            HasMany(c => c.Dialogs)
                .WithRequired(o => o.UserOne);
            HasMany(c => c.SentFriends)
                .WithRequired(o => o.UserBy);
            HasMany(c => c.ReceievedFriends)
                .WithRequired(o => o.UserTo);
            HasMany(c => c.Posts)
                .WithOptional(o => o.User);
        }
        
    }
}
