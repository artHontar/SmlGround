using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmlGround.DataAccess.Models;

namespace SmlGround.DLL.DTO
{
    public class UserDTO
    {
         
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string PlaceOfStudy { get; set; }
        public string Skype { get; set; }
        public string Role { get; set; }
        public ICollection<Friend> SentFriends { get; set; }
        public ICollection<Friend> ReceievedFriends { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
