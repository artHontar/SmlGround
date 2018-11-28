﻿using System;
using Common.Enum;

namespace SmlGround.DataAccess.Models
{
    
    public class Friend
    {
        public string UserById { get; set; }
        public string UserToId { get; set; }
        public User UserBy { get; set; }
        public User UserTo { get; set; }
        public DateTime CreationTime { get; set; }
        public FriendRequestFlag FriendRequestFlag { get; set; }
        public string Relationship { get; set; }
    }
}
