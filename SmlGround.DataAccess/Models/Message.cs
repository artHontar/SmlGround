using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Message
    {
        public long MyId { get; set; }
        public long? DialogId { get; set; }
        public virtual Dialog Dialog { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
