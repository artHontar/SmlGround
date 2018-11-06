using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Friend
    {
        public long FriendId { get; set; }
        public long? UserOneId { get; set; }
        public long? UserTwoId { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public DateTime CreationTime { get; set; }
        public string Relationship { get; set; }
    }
}
