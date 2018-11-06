using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Configuration
{
    class PostConfiguration : EntityTypeConfiguration<Post>
    {
        public PostConfiguration()
        {
            ToTable("Posts")
                .HasKey(o => o.PostId);
            Property(o => o.CreationTime)
                .IsRequired();
            Property(o => o.Text)
                .IsOptional().HasMaxLength(255);
            Property(o => o.Image)
                .IsOptional();
        }
    }
}
