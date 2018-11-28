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
        }

        public long DialogId { get; set; }
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
