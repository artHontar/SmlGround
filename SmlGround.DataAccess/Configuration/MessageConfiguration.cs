using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  SmlGround.DataAccess.Models;
using System.Data.Entity.ModelConfiguration;

namespace SmlGround.DataAccess.Configuration
{
    class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            ToTable("Messages").HasKey(o => o.MyId);
            Property(o => o.CreationTime)
                .IsRequired();
            Property(o => o.Text)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
