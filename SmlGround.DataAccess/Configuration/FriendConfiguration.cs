using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Configuration
{
    class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            ToTable("Friends");
            Property(o => o.CreationTime)
                .IsRequired();
            Property(o => o.Relationship)
                .IsOptional().HasMaxLength(54);
        }
    }
}
