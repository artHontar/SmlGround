using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using SmlGround.DataAccess.Models;

namespace SmlGround.DataAccess.Configuration
{
    class DialogConfiguration : EntityTypeConfiguration<Dialog>
    {
        public DialogConfiguration()
        {
            ToTable("Dialogs")
                .HasKey(o => o.DialogId);
            Property(o => o.CreationTime)
                .IsRequired();
            HasMany(c => c.Messages)
                .WithOptional(o => o.Dialog);

        }
    }
}
