using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Posts = new List<Post>();
            Dialogs = new List<Dialog>();
            SentFriends = new List<Friend>();
            ReceivedFriends = new List<Friend>();
            MyMessage = new List<Message>();
            ToMeMessage = new List<Message>();
        }


        public DateTime RegistrationTime { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Friend> SentFriends { get; set; }
        public ICollection<Friend> ReceivedFriends { get; set; }
        public virtual ICollection<Dialog> Dialogs { get; set; }

        public ICollection<Message> MyMessage { get; set; }
        public ICollection<Message> ToMeMessage { get; set; }

        public virtual Profile Profile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
