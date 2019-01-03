using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmlGround.Models
{
    public class MessageViewModel
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public bool Read { get; set; }
        public DateTime CreationTime { get; set; }

    }
}