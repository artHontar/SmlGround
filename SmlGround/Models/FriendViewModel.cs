using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Enum;
using SmlGround.DataAccess.Models;

namespace SmlGround.Models
{
    public class FriendViewModel
    {
        public string UserById { get; set; }
        public string UserToId { get; set; }
        public DateTime CreationTime { get; set; }
        public FriendStatus FriendRequestFlag { get; set; }
        public string Relationship { get; set; }
    }
}