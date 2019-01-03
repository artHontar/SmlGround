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
            ToTable("Messages").HasKey(o => o.MessageId);
            HasRequired(c => c.Sender).WithMany(o => o.MyMessage).HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false);
            HasRequired(c => c.Receiver).WithMany(o => o.ToMeMessage).HasForeignKey(m => m.ReceiverId)
                .WillCascadeOnDelete(false);

            Property(o => o.CreationTime)
                .IsRequired();
            Property(o => o.Text)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
