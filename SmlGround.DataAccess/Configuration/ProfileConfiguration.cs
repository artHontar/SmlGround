using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using SmlGround.DataAccess.Models;
namespace SmlGround.DataAccess.Configuration
{
    class ProfileConfiguration : EntityTypeConfiguration<Profile>
    {
        public ProfileConfiguration()
        {
           ToTable("Profiles")
                .HasKey(o => o.Id);
            Property(o => o.Name)
                .IsOptional();
            Property(o => o.Surname)
                .IsOptional();
            Property(o => o.Avatar)
                .IsOptional();
            Property(o => o.Birthday)
                .IsOptional();
           Property(o => o.City)
                .IsOptional()
                .HasMaxLength(54);
           Property(o => o.PlaceOfStudy)
                .IsOptional()
                .HasMaxLength(128);
           Property(o => o.Skype)
                .IsOptional()
                .HasMaxLength(54);
        }
    }
}
