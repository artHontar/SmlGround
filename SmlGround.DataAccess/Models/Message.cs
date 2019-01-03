﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Message
    {
        public long MessageId { get; set; }
        public long? DialogId { get; set; }
        public virtual Dialog Dialog { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public  User Sender { get; set; }
        public User Receiver { get; set; }
        public string Text { get; set; }
        public bool Read { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
