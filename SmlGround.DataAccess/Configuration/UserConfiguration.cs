using SmlGround.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using  Microsoft.AspNet.Identity.EntityFramework;

namespace SmlGround.DataAccess.Configuration
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("AspNetUsers")
                .HasKey(o => o.Id);
            Property(o => o.RegistrationTime)
                .IsRequired();
            HasRequired(c => c.Profile)
                .WithRequiredPrincipal(o => o.User);
            HasMany(c => c.Dialogs)
                .WithOptional(o => o.UserOne);
            HasMany(c => c.Friends)
                .WithOptional(o => o.UserOne);
            HasMany(c => c.Posts)
                .WithOptional(o => o.User);
        }
        
    }
}
