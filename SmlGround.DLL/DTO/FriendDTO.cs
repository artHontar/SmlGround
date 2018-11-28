using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
using SmlGround.DataAccess.Models;

namespace SmlGround.DLL.DTO
{
    public class FriendDTO
    {
        public string UserById { get; set; }
        public string UserToId { get; set; }
        public DateTime CreationTime { get; set; }
        public FriendRequestFlag FriendRequestFlag { get; set; }
        public string Relationship { get; set; }
    }
}
