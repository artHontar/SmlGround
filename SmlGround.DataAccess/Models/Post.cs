using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Post
    {
        public long PostId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
