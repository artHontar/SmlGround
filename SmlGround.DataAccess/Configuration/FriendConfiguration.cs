using SmlGround.DataAccess.Models;
using System.Data.Entity.ModelConfiguration;

namespace SmlGround.DataAccess.Configuration
{
    class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            ToTable("Friends");
            HasKey(o => new { o.UserById, o.UserToId });//
            HasRequired(c => c.UserBy).WithMany(o => o.SentFriends).HasForeignKey(m => m.UserById)
                .WillCascadeOnDelete(false);
            HasRequired(c => c.UserTo).WithMany(o => o.ReceievedFriends).HasForeignKey(m => m.UserToId)
                .WillCascadeOnDelete(false);
            //Property(o => o.UserOneId).IsRequired();
            //Property(o => o.UserTwoId).IsRequired();
            Property(o => o.CreationTime)
                .IsRequired();
            Property(o => o.Relationship)
                .IsOptional().HasMaxLength(54);
        }
    }
}
