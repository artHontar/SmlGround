using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Dialog
    {
        public Dialog()
        {
            Messages = new List<Message>();
            Users = new List<User>();
            
        }

        public long DialogId { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
