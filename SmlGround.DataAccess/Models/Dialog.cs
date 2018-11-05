using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Dialog
    {
        public long DialogId { get; set; }
        public long? User1Id { get; set; }
        public long? User2Id { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
